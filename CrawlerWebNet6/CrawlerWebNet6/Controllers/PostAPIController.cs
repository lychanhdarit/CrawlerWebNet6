using CrawlerWebNet6.Models;
using Microsoft.AspNetCore.Mvc; 

namespace CrawlerWebNet6.Controllers
{
    //[ApiController]
    public class PostAPIController : Controller
    {
        [Route("/post/get")]
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new BloggingContext())
            {
                Console.WriteLine($"Database path: {db.DbPath}.");
                // Read
                Console.WriteLine("Querying for a blog");
                var blogs = db.Posts
                    .OrderBy(b => b.PostId)
                    .ToList();
                return Ok(blogs);
            } 
        }
        [Route("/post/getinfo/{id}")]
        [HttpGet]
        public IActionResult GetInfoBlog(int id)
        {
            using (var db = new BloggingContext())
            {
 
                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Posts
                    .OrderBy(b => b.PostId)
                    .First(m=>m.PostId == id);
                return Ok(blog);
            }
            
        }
         
        
    }
}
