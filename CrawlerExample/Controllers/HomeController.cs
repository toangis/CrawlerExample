using CrawlerExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;

namespace CrawlerExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private StackContext _Stack;

        public HomeController(ILogger<HomeController> logger, StackContext Stack)
        {
            _logger = logger;
            _Stack = Stack;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult GetQuestionEntity()
        {
            List<Question> lstQuestion = _Stack.Question.Where(s => s.id > 0).ToList();
            return Json(JsonConvert.SerializeObject(lstQuestion));
        }

        public JsonResult LuuQuesion()
        {
            try
            {
                IEnumerable<Question> questions = Processor.GetQuestions();
                List<Question> lstQuestions = questions.ToList();
                for (int i = 0; i < lstQuestions.Count; i++)
                {
                    _Stack.Add(lstQuestions[i]);
                    _Stack.SaveChanges();
                }
                //string json = new JavaScriptSerializer().Serialize(questions);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }  
        }

        public IActionResult ShowStackQuesion()
        {
            return View();
        }

        public IActionResult ShowQuesionEntityFramework()
        {
            return View();
        }

        public IActionResult Crawler()
        {
            return View();
        }

        public IActionResult TableExample()
        {
            return View();
        }

        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 5, 10, 15
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (5,10,15)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IEnumerable<Question> questions = Processor.GetQuestions();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    questions = questions.OrderBy(s => s.Title);
                }
                //Tìm kiếm theo giá trị text gõ trong hộp input
                if (!string.IsNullOrEmpty(searchValue))
                {
                    questions = questions.Where(m => m.Title.Contains(searchValue) || m.Summary.Contains(searchValue));
                }

                //total number of rows count (tổng số dòng sẽ hiển thị ở bảng table)
                recordsTotal = questions.Count();
                //Paging 
                var data = questions.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                //return Json(new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult LoadDataFromEntityFramework()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //Lấy dữ liệu từ dbo.Question sử dụng LINQ
                List<Question> questions = _Stack.Question.ToList();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    questions = questions.OrderBy(s => s.Title).ToList();
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    questions = questions.Where(m => m.Title.Contains(searchValue) || m.Summary.Contains(searchValue)).ToList();
                }

                recordsTotal = questions.Count();
                var data = questions.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
