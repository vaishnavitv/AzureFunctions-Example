using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions
{
    public static class TODOService
    {

        static Dictionary<int, string> TODOList = new Dictionary<int, string>();

        static TODOService()
        {
            TODOList.Add(1, "Make tea");
            TODOList.Add(2, "Do Laundry");
        }

        public static HttpResponseMessage DoGet(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("In GET");

            //id is queryParam
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0)
                .Value;

            if (id == null)
            {
                return req.CreateResponse(TODOList.Values);
            }

            return req.CreateResponse(TODOList[Convert.ToInt32(id)]);
        }

        public static HttpResponseMessage DoPut(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("In PUT");

            //id is queryParam, value is body itself
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0)
                .Value;

            if (id == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Please set id Parameter");
            }

            string value = req.Content.ReadAsStringAsync().Result;

            if (value == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Please set value in Body");
            }

            TODOList[Convert.ToInt32(id)] = value;

            return req.CreateResponse(id);
        }

        public static HttpResponseMessage DoPost(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("In POST");

            //id is queryParam, value is body itself
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0)
                .Value;

            if (id == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Please set id Parameter");
            }

            string value = req.Content.ReadAsStringAsync().Result;

            if (value == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Please set value in Body");
            }

            TODOList.Add(Convert.ToInt32(id), value);

            return req.CreateResponse(id);
        }

        public static HttpResponseMessage DoDelete(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("In DELETE");
            
            //id is queryParam
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0)
                .Value;

            if (id == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Please set id Parameter");
            }

            TODOList.Remove(Convert.ToInt32(id));

            return req.CreateResponse(id);
        }

        public static HttpResponseMessage DoUnknown(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("In Unknown");
            return req.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "Method not allowed");
        }

        [FunctionName("TODO")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete",  Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("TODOService: C# HTTP trigger function processed a request.");

            if (req.Method.Equals(HttpMethod.Get))
            {
                return DoGet(req, log);
            }
            else if (req.Method.Equals(HttpMethod.Post))
            {
                return DoPost(req, log);
            }
            else if (req.Method.Equals(HttpMethod.Put))
            {
                return DoPut(req, log);
            }
            else if (req.Method.Equals(HttpMethod.Delete))
            {
                return DoDelete(req, log);
            }
            else
            {
                return DoUnknown(req, log); //Currently, all methods are defined above in the signature, won't get called.
            }
        }
    }
}
