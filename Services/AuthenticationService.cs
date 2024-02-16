using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StorageUI.Data;
using StorageUI.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;


namespace StorageUI.Services
{
    public interface IAppAuthenticationService
    {
        Task<bool> Authenticate(string username, string password);
        Task<UserApp> GetUserAsync();
    }

    public class AuthenticationService : IAppAuthenticationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

        public AuthenticationService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }

        public async Task<bool> Authenticate(string username, string password)
        {

            //password = BCrypt.Net.BCrypt.HashPassword(password);

            var user = await _dbContext.UserApp.FirstOrDefaultAsync(u => u.Name == username);
            // Substitua o trecho abaixo com a verificação de senha apropriada

            if (user != null && VerifyPassword(password, user.Password))
            {
                _customAuthenticationStateProvider.MarkUserAsAuthenticated(username);
                return true;
            }

            return false;
        }

        public async Task<UserApp> GetUserAsync()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrEmpty(username))
            {
                var user = await _dbContext.UserApp.FirstOrDefaultAsync(u => u.Name == username);
                return user;
            }
            return null;
        }

        // Substitua este método com sua lógica de hash de senha
        private bool VerifyPassword(string providedPassword, string storedPasswordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                // Log the exception details to investigate the invalid salt format
                Console.WriteLine(ex.Message);
                // Consider additional logic here, like flagging the issue for review
                return false;
            }
        }

    }
}
