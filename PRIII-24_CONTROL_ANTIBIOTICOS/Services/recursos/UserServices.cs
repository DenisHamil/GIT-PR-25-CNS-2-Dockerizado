using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services;


namespace PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos
{
    public class UserService : IUserService
    {
        private readonly BdProa1Context _dbContext;
        public UserService(BdProa1Context dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<User> GetUser(string username, string password)
        {
            User user_encontrado = await _dbContext.Users.Where(u => u.Username == username && u.Password == password)
                    .FirstOrDefaultAsync();
            return user_encontrado;

        }

    }
}
