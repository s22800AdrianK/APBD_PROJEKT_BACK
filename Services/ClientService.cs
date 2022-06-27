using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Services
{
    public class ClientService : IClientService
    {
        private readonly DBcontext _context;
        private readonly IConfiguration _configuration;
        public ClientService(DBcontext DBContext, IConfiguration configuration)
        {
            _context = DBContext;
            _configuration = configuration;
        }
        public bool CheckPasswordLength(string password)
        {
            return (password.Length <= 100); 
        }

        public async Task<bool> DoesPasswordMatch(Models.DTOs.Client client)
        {
            var c = await _context.Clients.FirstOrDefaultAsync(e => e.Login == client.Login);
            return client.Password == getHashedSaltedPassword(client.Password, c.Salt);
        }

        public async Task<bool> DoesRefreashTokenMatch(string refreashToken)
        {
            return await _context.Clients.AnyAsync(e => e.RefreshToken == refreashToken);
        }

        public JwtSecurityToken GetOptions(int idUser)
        {
            Claim[] userClaims = new[]
            {
                new Claim(ClaimTypes.Role, "client"),
                new Claim("client",idUser.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );

            return token;
        }


        public async Task<Models.Client> GetRefrashedToken(string token)
        {
            var c = await _context.Clients.FirstOrDefaultAsync(e => e.RefreshToken == token);
            c.RefreshToken = getRefreshToken();
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<bool> IsLogginUnique(string login)
        {
            return await _context.Clients.AnyAsync(e => e.Login == login);
        }

        public async Task<Models.Client> Login(Models.DTOs.Client client)
        {
            var c = await _context.Clients.FirstOrDefaultAsync(e => e.Login == client.Login);
            c.RefreshToken = getRefreshToken();
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task RegisterNewClient(Models.DTOs.Client client)
        {
            var salt = getSalt(client.Password);
            var pass = getHashedPassword(client.Password, salt.Value);
            var token = getRefreshToken();
            var c = new Client
            {
                Login = client.Login,
                Password = pass,
                Salt = salt.Key,
                RefreshToken = token,
                Email = client.Email
            };

            await _context.Clients.AddAsync(c);
            await _context.SaveChangesAsync();
        }

        public KeyValuePair<string, byte[]> getSalt(string password)
        {
            byte[] saltBytes = new byte[50];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(saltBytes);
            return new KeyValuePair<string, byte[]>(Convert.ToBase64String(saltBytes), saltBytes);
        }

        public string getHashedPassword(string password, byte[] saltBytes)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32));
        }

        public string GetHashedSaltedPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            var hashedSaltedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hashedSaltedPassword;
        }

        public string getRefreshToken()
        {
            byte[] tokenBytes = new byte[1000];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        public string getHashedSaltedPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            var hashedSaltedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hashedSaltedPassword;
        }

        public async Task<bool> DoesClientExists(int id)
        {
            return await _context.Clients.AnyAsync(e => e.IdUser == id);
        }
    }
}
