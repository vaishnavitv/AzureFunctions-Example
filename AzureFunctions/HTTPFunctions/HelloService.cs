using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions
{
    public static class HelloService
    {
        [FunctionName("Hello")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Hello: C# HTTP trigger function processed a request.");

            // parse query parameter
            string firstName = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "FirstName", true) == 0)
                .Value;

            string lastName = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "LastName", true) == 0)
                .Value;

            if (firstName == null || lastName == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                firstName = data?.FirstName;
                lastName = data?.LastName;
            }

            if (firstName == null || lastName == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
            }

            return req.CreateResponse(HttpStatusCode.OK, string.Format("Hello {0} {1}!",firstName, lastName));
        }
    }
}
