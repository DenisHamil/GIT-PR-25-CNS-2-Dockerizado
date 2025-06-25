using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos
{
        public interface IUserService
        {
            Task<User> GetUser(string username, string password);

        } 
}
