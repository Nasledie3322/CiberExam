using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class RequestTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            Console.WriteLine($"Request took {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
