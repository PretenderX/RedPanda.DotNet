using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RedPanda.Service.HealthCheck.WebApi
{
    [RoutePrefix("api/healthcheck")]
    public class HealthCheckController : ApiController
    {
        private readonly IHealthCheckExecutor healthCheckExecutor;

        public HealthCheckController(IHealthCheckExecutor healthCheckExecutor)
        {
            this.healthCheckExecutor = healthCheckExecutor;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Index()
        {
            if (healthCheckExecutor == null)
            {
                return Ok(new HealthCheckResult());
            }

            var healthCheckResult = await healthCheckExecutor.ExecuteAsync();

            return Content(healthCheckResult.IsHealthy ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable, healthCheckResult);
        }
    }
}
