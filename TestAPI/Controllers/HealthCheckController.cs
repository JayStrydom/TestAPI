using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace TestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly PerformanceContext _context;

        public HealthCheckController(PerformanceContext context)
        {
            _context = context;
        }

        // GET: healthcheck
        [HttpGet]
        public IActionResult HealthCheck()
        {
            var model = new SessionModel();
            model.hostname = Dns.GetHostName();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var sql = string.Format(@"
                    SELECT[host_name], COUNT(session_id) As NumberOfSessions
                    FROM sys.dm_exec_sessions
                    WHERE original_login_name = 'demo' AND host_name = '{0}'
                    GROUP BY[host_name]
                    ORDER BY COUNT(session_id) DESC", model.hostname);

                command.CommandText = sql;
                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    model.sessions = (int)reader[1];
                }
            }

            const int minSessions = 10;

            if (model.sessions < minSessions)
            {
                // 503 - Service Not Ready
                model.ready = false;
                return StatusCode(503, model);
            }

            return Ok(model);
        }

        public class SessionModel
        {
            public string hostname { get; set; }
            public int sessions { get; set; }
            public bool ready { get; set; } = true;
        }
    }
}
