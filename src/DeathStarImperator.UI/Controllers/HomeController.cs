using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeathStarImperator.UI.Hubs;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

    public class JobManager
    {
        public static readonly JobManager Instance = new JobManager();

        public JobManager()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
        }
        ConcurrentDictionary<string, Job> _runningJobs = new ConcurrentDictionary<string, Job>();
        private IHubContext _hubContext;

        public Job DoJobAsync(Action<Job> action)
        {
            var job = new Job(Guid.NewGuid().ToString());

            // this will (should!) never fail, because job.Id is globally unique
            _runningJobs.TryAdd(job.Id, job);

            Task.Factory.StartNew(() =>
            {
                action(job);
                job.ReportComplete();
                _runningJobs.TryRemove(job.Id, out job);
            },
            TaskCreationOptions.LongRunning);

            BroadcastJobStatus(job);

            return job;
        }

        private void BroadcastJobStatus(Job job)
        {
            job.ProgressChanged += HandleJobProgressChanged;
            job.Completed += HandleJobCompleted;
        }

        private void HandleJobCompleted(object sender, EventArgs e)
        {
            var job = (Job)sender;

            _hubContext.Clients.Group(job.Id).jobCompleted(job.Id);

            job.ProgressChanged -= HandleJobProgressChanged;
            job.Completed -= HandleJobCompleted;
        }

        private void HandleJobProgressChanged(object sender, EventArgs e)
        {
            var job = (Job)sender;
            _hubContext.Clients.Group(job.Id).progressChanged(job.Id, job.Progress);
        }

        public Job GetJob(string id)
        {
            Job result;
            return _runningJobs.TryGetValue(id, out result) ? result : null;
        }
    }

    public class Job
    {
        public event EventHandler<EventArgs> ProgressChanged;
        public event EventHandler<EventArgs> Completed;

        private volatile int _progress;
        private volatile bool _completed;
        private CancellationTokenSource _cancellationTokenSource;

        public Job(string id)
        {
            Id = id;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; private set; }

        public int Progress
        {
            get { return _progress; }
        }

        public bool IsComplete
        {
            get { return _completed; }
        }

        public CancellationToken CancellationToken
        {
            get { return _cancellationTokenSource.Token; }
        }

        public void ReportProgress(int progress)
        {
            if (_progress != progress)
            {
                _progress = progress;
                OnProgressChanged();
            }
        }

        public void ReportComplete()
        {
            if (!IsComplete)
            {
                _completed = true;
                OnCompleted();
            }
        }

        protected virtual void OnCompleted()
        {
            var handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnProgressChanged()
        {
            var handler = ProgressChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}