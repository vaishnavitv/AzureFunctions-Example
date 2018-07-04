using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions
{
    public static class RemoveVowels
    {
        [FunctionName("RemoveVowels")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("RemoveVowels: C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = null;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data;
            }

            if (name == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            var charsToRemove = new string[] { "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            foreach (var c in charsToRemove)
            {
                name = name.Replace(c, string.Empty);
            }

            return req.CreateResponse(name);
        }
    }
}
