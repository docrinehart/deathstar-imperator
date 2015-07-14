using System.Threading;
using System.Web.Mvc;
using DeathStarImperator.UI.Models;

namespace DeathStarImperator.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult ProgressDemo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoJob()
        {
            var job = JobManager.Instance.DoJobAsync(j =>
            {
                for (var progress = 0; progress <= 100; progress++)
                {
                    Thread.Sleep(200);
                    j.ReportProgress(progress);
                }
            });

            return Json(new
            {
                JobId = job.Id,
                Progress = job.Progress
            });
        }
    }

    
}