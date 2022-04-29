using CrawlerWebNet6.Models;

namespace CrawlerWebNet6.Services
{
    public class CrawlerJobs
    {

        public static void CrawlVNExpress()
        {
            try
            {
                Console.WriteLine($"{DateTime.Now} Begin crawl...");
                var list = GetUrls("https://vnexpress.net/the-thao");
                foreach (var item in list)
                {
                    Console.WriteLine($"Runing: {item.Link} ");
                    var post = GetPost(item.Link);
                    if (post != null)
                    {
                        Console.WriteLine($" {post.Title} ");

                        using (var db = new BloggingContext())
                        {
                            Console.WriteLine($"Database path: {db.DbPath}.");
                            var exist = db.Posts.FirstOrDefault(m => m.Title.Contains(post.Title));
                            if (exist == null)
                            {
                                // Create 
                                db.Add(post);
                                db.SaveChanges();
                            }

                        }
                        Console.WriteLine($"----------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: Cần tạo danh mục");
            }
        }
        public static List<LinkCategoryModel> GetUrls(string categoryUrl)
        { 
            var htmlHelper = new HtmlAgilityHelpers();
            var doc = htmlHelper.GetDocument(categoryUrl); 
            var list = htmlHelper.GetLinkAndTextFromElement(doc, "//article//h3[@class='title-news']//a"); 
            return list; 
        }
        public static Post GetPost(string url)
        {
            var post = new Post();
            var htmlHelper = new HtmlAgilityHelpers();
            var doc = htmlHelper.GetDocument(url);
            if (doc != null)
            {
                post.Title = htmlHelper.GetTitle(doc);
                var desc = htmlHelper.GetDescription(doc); 
                string tagAttribute = "//article[@class='fck_detail ']";
                post.Content = htmlHelper.GetTextOnlyFromElement(doc,tagAttribute,true);
                post.BlogId = 1;
            }
            return post;
        }
    }
}
