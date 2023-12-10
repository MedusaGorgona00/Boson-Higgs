using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class IpRequestThrottlingAttribute : ActionFilterAttribute
{
    private readonly int requestLimit;
    private readonly int interval;
    private readonly ConcurrentDictionary<string, int> ipRequestCounts = new ConcurrentDictionary<string, int>();
    private readonly ConcurrentDictionary<string, DateTime> ipRequestTime = new ConcurrentDictionary<string, DateTime>();

    public IpRequestThrottlingAttribute(int requestLimit, int interval)
    {
        this.requestLimit = requestLimit;
        this.interval = interval;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        if (!string.IsNullOrEmpty(ipAddress))
        {
            int requestCount = ipRequestCounts.AddOrUpdate(ipAddress, 1, (key, value) => value + 1);
            if (requestCount == 1)
            {
                ipRequestTime.TryAdd(ipAddress, DateTime.Now);
            }
            else if (requestCount > requestLimit)
            {
                ipRequestTime.TryGetValue(ipAddress, out var time);
                if (time != null)
                {
                    if (time.AddMinutes(interval).CompareTo(DateTime.Now) > 0)
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        context.Result = new ContentResult
                        {
                            Content = $"Too Many Requests. You have only {requestLimit} requests per {interval} minute."
                        };
                        return;
                    }
                    else
                    {
                        ipRequestTime.TryUpdate(ipAddress, DateTime.Now, time);
                        ipRequestCounts.TryRemove(ipAddress, out _);
                    }
                }

            }
        }

        base.OnActionExecuting(context);
    }
}
