using CrawlerWebNet6.Models;
using CrawlerWebNet6.Services;
using Hangfire;
using Hangfire.MemoryStorage; 
using Microsoft.OpenApi.Models;

string MyOrigin = "AllowMyOrigin";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();


builder.Services.AddSingleton<BloggingContext>();


builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMemoryStorage());

        builder.Services.AddHangfireServer(); 

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dữ liệu  ",
        Description = "Microservices ",
        TermsOfService = new Uri("https://vinapage.vn"),
        Contact = new OpenApiContact
        {
            Name = "RIL LY",
            Email = "lychanhdaric@gmail.com",
            Url = new Uri("https://vinapage.vn"),
        },
        License = new OpenApiLicense
        {
            Name = "Phone: 0964.900.534",
            Url = new Uri("https://vinapage.vn"),
        }
    });
});


var corsSetting = builder.Configuration.GetSection("Cors");
string[] origins = { };
if (corsSetting != null)
{
    if (!string.IsNullOrEmpty(corsSetting.Value))
    {
        origins = corsSetting.Value.Split(',');
    }
}
if (origins.Count() > 0)
{
    builder.Services.AddCors(c =>
    {
        c.AddPolicy(MyOrigin, builder => builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
    });
}
else
{
    builder.Services.AddCors(c =>
    {
        c.AddPolicy(MyOrigin, builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials());
    });
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();  

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
});




var backgroundJobs = new BackgroundJobClient();
backgroundJobs.Enqueue(() => CrawlerJobs.CrawlVNExpress()); 

var recurringJob = new RecurringJobManager();
//Cron S M H Dayofmonth Month Dayofweek Year 
recurringJob.AddOrUpdate("Run Recurring Job", () => CrawlerJobs.CrawlVNExpress(), Cron.MinuteInterval(30));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");  

    endpoints.MapRazorPages();
    endpoints.MapHangfireDashboard();
});





app.Run();
