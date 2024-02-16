using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            // Adicione outras claims necessárias aqui, como roles
        }, "apiauth"); // Esquema de autenticação

        var userPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
    }

    public void MarkUserAsLoggedOut()
    {
        var identity = new ClaimsIdentity(); // Cria um principal vazio
        var userPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Retorna um estado de autenticação padrão com um usuário não autenticado
        var identity = new ClaimsIdentity();
        var userPrincipal = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(userPrincipal));
    }
}
