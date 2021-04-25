using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using communication.API.Models;


namespace communication.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null) return null;
            if (!VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash))
                return null;


            return user;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmack = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var ComputedPassworddHash = hmack.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < ComputedPassworddHash.Length; i++)
                {
                    if (ComputedPassworddHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Using(){} To Dispose Method After Generating Password Hash
            using (var hmack = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmack.Key;
                passwordHash = hmack.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == username)) return true;
            return false;
        }
    }
}