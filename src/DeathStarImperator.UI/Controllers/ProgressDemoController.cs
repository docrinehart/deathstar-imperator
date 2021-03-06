﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using DeathStarImperator.UI.Models;

namespace DeathStarImperator.UI.Controllers
{
    public class ProgressDemoController : Controller
    {
        private readonly IProgressJobGenerator _progressJobGenerator ;

        public ProgressDemoController(IProgressJobGenerator progressJobGenerator)
        {
            _progressJobGenerator = progressJobGenerator;
        }

        public ActionResult ProgressDemo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoJob()
        {
            var job = _progressJobGenerator.Execute(j =>
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