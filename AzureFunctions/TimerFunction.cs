using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions
{
    public static class TimerFunction
    {
        [FunctionName("TimerFunction")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
