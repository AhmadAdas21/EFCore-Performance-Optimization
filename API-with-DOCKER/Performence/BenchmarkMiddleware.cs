
using System.Diagnostics;

namespace API_with_DOCKER.Performance
{
    public class BenchmarkMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BenchmarkMiddleware> _logger;

        public BenchmarkMiddleware(RequestDelegate next,ILogger<BenchmarkMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, SqlQueryCounter counter)
        {
            if (!context.Request.Path.StartsWithSegments("/api/orders/report"))
            {
                await _next(context);
                return;
            }
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation("BENCHMARK | Path: {Path} | SQL Reader Commands: {Count} | Execution Time: {Time} ms",
                    context.Request.Path,
                    counter.Count,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
