
namespace HRMSPOC.WEB.Controllers
{
    public class AuthHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHttpClientHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("JWTToken");
            if(!string.IsNullOrEmpty(token) )
            {
                Console.WriteLine($"Token attached to request: {token}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                Console.WriteLine("Token is missing from the session.");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
