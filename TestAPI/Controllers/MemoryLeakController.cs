using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace TestAPI.Controllers
{
    // Examples: https://github.com/sebastienros/memoryleak

    [Route("[controller]")]
    [ApiController]
    public class MemoryLeakController : ControllerBase
    {

        /// <summary>
        /// Transient example: memory grows temporary until the GC collection gen 0 instances about every 2 sec
        /// Note: as long as the CPU is not over-utilized, the garbage collection can deal with a high number of allocations
        /// </summary>
        [HttpGet("transient")] // memoryleak/transient
        public ActionResult<string> GetTransient()
        {
            return new String('x', 1000 * 1024);
        }

        private static ConcurrentBag<string> _staticStrings = new ConcurrentBag<string>();

        /// <summary>
        /// Managed memory leak example: new objects are held forever so that the GC can't release it
        /// </summary>
        [HttpGet("managed")] // memoryleak/managed
        public ActionResult<string> GetManaged()
        {
            var bigString = new String('x', 1000 * 1024);
            _staticStrings.Add(bigString);
            return bigString;
        }

    }
}
