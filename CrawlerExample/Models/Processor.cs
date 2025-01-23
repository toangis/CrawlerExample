using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerExample.Models
{
    public class Processor
    {
        public static IEnumerable<Question> GetQuestions()
        {
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            var result = new List<Question>();
            int stt = 0;
            for (int i = 14; i < 19; i++)
            {
                //Crawler trang web https://stackoverflow.com/questions?tab=newest&page= 
                var document = web.Load("https://stackoverflow.com/questions?tab=newest&page=" + i);
                //Lấy tất cả các thẻ div có class = s-post-summary nằm trong thẻ div có id = questions
                var questions = document.DocumentNode
                    .QuerySelectorAll("div#questions > div.s-post-summary").ToList();

                //Lặp qua từng trang và lấy thông tin các bài viết trong từng trang
                //Ví dụ 1 trang có 15 bài viết => có tổng cộng 5 * 15 = 75 bài viết
                foreach (var question in questions)
                {
                    stt++;
                    var title = question.QuerySelectorAll("div.s-post-summary--content > h3 > a").FirstOrDefault().InnerText;
                    
                    var sumary = question.QuerySelectorAll("div.s-post-summary--content > div.s-post-summary--content-excerpt")
                        .FirstOrDefault().InnerText.Replace("\r\n", "").Trim();

                    var tempAnws = question.QuerySelectorAll("div.js-post-summary-stats > div.s-post-summary--stats-item").ElementAt(1);
                    string tAnws = tempAnws.QuerySelectorAll("span").FirstOrDefault().InnerText;
                    var anws = int.Parse(tAnws);

                    var tempLast = question.QuerySelectorAll("div.js-post-summary-stats > div.s-post-summary--stats-item").Last();
                    string tViews = tempLast.QuerySelectorAll("span").FirstOrDefault().InnerText;
                    var views = int.Parse(tViews);

                    var votes = int.Parse(question.QuerySelectorAll("div.s-post-summary--stats > div.s-post-summary--stats-item__emphasized > span")
                        .FirstOrDefault().InnerText);

                    result.Add(new Question()
                    {
                        Title = title,
                        Summary = sumary,
                        Answers = anws,
                        Views = views,
                        Votes = votes
                    });
                }
            }
            return result;
        }
    }
}
