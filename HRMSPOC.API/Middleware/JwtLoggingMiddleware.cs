namespace HRMSPOC.API.Middleware
{
    public class JwtLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtLoggingMiddleware> _logger;
        public JwtLoggingMiddleware(RequestDelegate next,ILogger<JwtLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if(string.IsNullOrEmpty(token) )
            {
                _logger.LogWarning("Authorization Header is missing");
            }
            else
            {
                _logger.LogInformation($"Authorization Header received : {token}");
            }
            await _next(context);
        }
    }
}
