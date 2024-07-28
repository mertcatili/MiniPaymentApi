using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MiniPaymentApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _authToken;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
            _authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var token) || !ValidateToken(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }

        private bool ValidateToken(string token)
        {
            return token == _authToken;
        }
    }
}
