using Projekt.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Services
{
    public interface IStockService
    {
        public Task<List<Company>> GetCompanies(string regex);
        public Task<List<Company>> GetCompaniesDB(string regex);
        public Task<CompanyDetailed> GetCompany(string ticker);
        public Task<CompanyDetailed> GetCompanyDB(string ticker);
        public Task<List<OHLC>> GetOHLCs(string ticker, DateTime from, DateTime to, string timespan);
        public Task<List<OHLC>> GetOHLCsDB(string ticker, DateTime from, DateTime to);
        public bool isFromSmallerThanTo(DateTime from, DateTime to);
        public Task addOHLCsToDB(string ticker, List<OHLC> oHLCs);
        public Task<bool> DoesTickerExist(string ticker);
        public Task<bool> DoesTickerExistDB(string ticker);
        public Task<bool> DoesWatchedElementExistinDB(string ticker);
        public Task<List<CompanyDetailed>> GetWatchedCompanies(int IdUser);
        public Task AddtoWatchlist(string ticker, int IdUser);
        public Task DeleteWatchlist(string ticker, int IdUser);
        public Task AddWatchElementDB(CompanyDetailed company);
        public Task SaveChangesAsync();
        public Task<bool> DeleteFromWatchList(string ticker, int IdUser);
        public Task<string> Test();
    }
}
