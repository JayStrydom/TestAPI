using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {

        // GET: threads
        [HttpGet()]
        public ThreadInfoModel GetThreadPools()
        {
            int minWorker, minIOC, maxWorker, maxPort;
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            ThreadPool.GetMaxThreads(out maxWorker, out maxPort);
                        
            return new ThreadInfoModel()
            {
                minWorkerThreads = minWorker,
                minCompletionPortThreads = minIOC,
                maxWorkerThreads = maxWorker,
                maxCompletionPortThreads = maxPort,
                processors = Environment.ProcessorCount
            };
        }

        public class ThreadInfoModel
        {
            public int processors { get; set; }
            public int minWorkerThreads { get; set; }
            public int minCompletionPortThreads { get; set; }
            public int maxWorkerThreads { get; set; }
            public int maxCompletionPortThreads { get; set; }
        }

        public class ThreadUpdateModel
        {
            public int minWorkerThreads { get; set; }
            public int minCompletionPortThreads { get; set; }
            public int maxWorkerThreads { get; set; }
            public int maxCompletionPortThreads { get; set; }
        }

        public class ThreadUpdateModelResult
        {
            public bool maxUpdated { get; set; }
            public bool minUpdated { get; set; }
        }

        // PUT: threads
        [HttpPut]
        public async Task<ThreadUpdateModelResult> SetThreads(ThreadUpdateModel model)
        {
            var result = new ThreadUpdateModelResult();

            if (model.minWorkerThreads > 0)
            {
                result.minUpdated = ThreadPool.SetMinThreads(model.minWorkerThreads, model.minCompletionPortThreads);
            }
            if (model.maxWorkerThreads > 0)
            {
                result.maxUpdated = ThreadPool.SetMaxThreads(model.maxWorkerThreads, model.maxCompletionPortThreads);
            }

            return result;
        }
    }
}
