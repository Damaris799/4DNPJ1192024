using _4DNPJ1192024.Auth;

namespace _4DNPJ1192024.Endpoints
{

    public static class AccountEndpoint
    {
        public static void AddAccountEndpoints(this WebApplication app)
        {
            app.MapPost("/account/login", (string login, string password,  IJwAuthenticationService authService) =>
            {
                if (login == "admin" && password == "12345")
                {
                    var token = authService.Authenticate(login);

                    return Results.Ok(token);
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

        }

    }
}
