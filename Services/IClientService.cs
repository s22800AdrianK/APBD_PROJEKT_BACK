using Projekt.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Services
{
    public interface IClientService
    {
        public Task<bool> IsLogginUnique(string login);
        public bool CheckPasswordLength(string password);
        public Task RegisterNewClient(Models.DTOs.Client client);
        public Task<bool> DoesPasswordMatch(Models.DTOs.Client client);
        public Task<bool> DoesRefreashTokenMatch(string refreashToken);
        public Task<bool> DoesClientExists(int id);
        public Task<Client> Login(Models.DTOs.Client client);
        public Task<Client> GetRefrashedToken(string token);
        public JwtSecurityToken GetOptions(int idUser);
    }
}
