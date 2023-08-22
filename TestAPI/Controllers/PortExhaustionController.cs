using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    // Examples: https://github.com/sebastienros/memoryleak

    [Route("[controller]")]
    [ApiController]
    public class PortExhaustionController : ControllerBase
    {

        /// <summary>
        /// Create the HTTP object every time instead of reusing it, leading to port exhaustion
        /// </summary>
        [HttpGet()] // portexhaustion?url=https://www.google.com
        public async Task<int> GetTransient([FromQuery] string url)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(url);
                return (int)result.StatusCode;
            }
        }
    }
}
