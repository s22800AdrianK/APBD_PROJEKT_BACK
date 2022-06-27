using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekt.Models;
using Projekt.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projekt.Services
{
    public class StockService : IStockService
    {
        private  readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly DBcontext _context;
        private readonly string API_KEY;
        public StockService(DBcontext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            API_KEY = _configuration["API_KEY"];
        }

        public Task<bool> DeleteFromWatchList(string ticker, int IdUser)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoesTickerExist(string ticker)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            try
            {
                var streamTask = client.GetStreamAsync($"https://api.polygon.io/v3/reference/tickers?active=true&sort=ticker&order=asc&limit=100&apiKey=1p7pfT07fHfgmupzsl4vHGteG2tEZMh7&ticker={ticker}");
                var t = await streamTask;
                var res = await JsonSerializer.DeserializeAsync<Results>(await streamTask);
                return res.results.Count() == 1;
            }
            catch (Exception)
            {
                return await _context.watchedElements.AnyAsync(e => e.ticker == ticker);
            }
        }

        public async Task<bool> DoesWatchedElementExistinDB(string ticker)
        {
            return await _context.watchedElements.AnyAsync(e => e.ticker == ticker);
        }

        public async Task<List<Company>> GetCompanies(String regex)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var streamTask = client.GetStreamAsync($"https://api.polygon.io/v3/reference/tickers?active=true&sort=ticker&order=asc&limit=100&apiKey={API_KEY}&ticker.gte={regex}");
            var res =  await JsonSerializer.DeserializeAsync<Results>(await streamTask);
            return res.results;
        }

        public async Task<string> Test()
        {
            var streamTask = client.GetStringAsync($"https://api.polygon.io/v3/reference/tickers?active=true&sort=ticker&order=asc&limit=100&apiKey={API_KEY}&ticker.gte=TS");
            return await streamTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddtoWatchlist(string ticker, int IdUser)
        {
            await _context.watchlists.AddAsync(new Watchlist { 
                ticker = ticker,
                IdUser = IdUser
            });
            await SaveChangesAsync();
        }

        public async Task AddWatchElementDB(CompanyDetailed company)
        {
            await _context.watchedElements.AddAsync(
                    new WatchedElement
                    {
                        logo = company.branding==null? "" : company.branding.logo_url,
                        Name = company.name,
                        ticker = company.ticker,
                        country = company.locale,
                        description = company.description==null? "" : company.description
                    }
                    );
            await SaveChangesAsync();
        }

        public async Task<CompanyDetailed> GetCompany(string regex)
        {
                client.DefaultRequestHeaders.Accept.Clear();
                var streamTask = client.GetStreamAsync($"https://api.polygon.io/v3/reference/tickers/{regex}?apiKey={API_KEY}");
                var res = await JsonSerializer.DeserializeAsync<ResultsDetailed>(await streamTask);

                if (!await DoesWatchedElementExistinDB(res.results.ticker)) await AddWatchElementDB(res.results);
                return res.results;
                    
        }

        public async Task<List<Models.DTOs.OHLC>> GetOHLCs(string ticker, DateTime from, DateTime to, string timespan)
        { 
            client.DefaultRequestHeaders.Accept.Clear();
            var toS = to.ToString("yyyy-MM-dd");
            var fromS = from.ToString("yyyy-MM-dd");
            var streamTask = client.GetStreamAsync($"https://api.polygon.io/v2/aggs/ticker/{ticker}/range/1/{timespan}/{fromS}/{toS}?adjusted=true&sort=asc&limit=120&apiKey={API_KEY}");
            var res1 = await JsonSerializer.DeserializeAsync<ResultsOHLC>(await streamTask);
            await addOHLCsToDB(ticker, res1.results);
            return res1.results;
        }

        public async Task<List<CompanyDetailed>> GetWatchedCompanies(int IdUser)
        {
            return await _context.watchlists.Where(e => e.IdUser == IdUser).Select(e => new CompanyDetailed
            {
                locale = e.WatchedElement.country,
                name = e.WatchedElement.Name,
                description = e.WatchedElement.description,
                ticker = e.WatchedElement.ticker,
                branding = new Logo { logo_url = e.WatchedElement.logo }
            }).ToListAsync();
        }

        public bool isFromSmallerThanTo(DateTime from, DateTime to)
        {
            return from <= to;
        }

        public async Task addOHLCsToDB(string ticker, List<Models.DTOs.OHLC> oHLCs)
        {
           var toDB = oHLCs.Where(o => !_context.oHLCs.Where(o1 => o1.WatchedElementTicker == ticker).Any(o1 => o.t == o1.t));
            foreach(var t in toDB)
            {
                await _context.AddAsync(
                    new Models.OHLC
                    {
                        WatchedElementTicker = ticker,
                        c = t.c,
                        t = t.t,
                        h = t.h,
                        l = t.l,
                        n = t.n,
                        o = t.o,
                        v = t.v,
                        vw = t.vw
                    }
                  );
            }
            await SaveChangesAsync();
        }

        public async Task<List<Company>> GetCompaniesDB(string regex)
        {
            return await _context.watchedElements.Where(e => e.ticker.Contains(regex))
                .Select(e => new Company
                {
                    ticker = e.ticker,
                    name = e.Name
                }).ToListAsync();
        }

        public async Task<CompanyDetailed> GetCompanyDB(string ticker)
        {
            var w = await _context.watchedElements.FirstOrDefaultAsync(e => e.ticker == ticker);
            if (w != null)
            return new CompanyDetailed { ticker = w.ticker, branding = new Logo { logo_url = w.logo }, locale = w.country, name = w.Name, description = w.description };
            else return null;
        }

        public async Task<List<Models.DTOs.OHLC>> GetOHLCsDB(string ticker, DateTime from, DateTime to)
        {
            var fromT = ToUnixTimeStamp(from);
            var ToT = ToUnixTimeStamp(to);
            return await _context.oHLCs.Where(e => e.WatchedElementTicker == ticker)
                .Where(e => e.t >= fromT && e.t <= ToT).Select(e => new Models.DTOs.OHLC
                {
                    c = e.c,
                    t = e.t,
                    h = e.h,
                    l = e.l,
                    n = e.n,
                    o = e.o,
                    vw = e.vw,
                    v = e.v
                }).ToListAsync();
        }

        public int ToUnixTimeStamp(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public async Task<bool> DoesTickerExistDB(string ticker)
        {
            return await _context.watchedElements.AnyAsync(e => e.ticker == ticker);
        }

        public async Task DeleteWatchlist(string ticker, int IdUser)
        {
            var wl = new Watchlist { IdUser = IdUser, ticker = ticker };
            var entry = _context.Entry(wl);
            entry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> doesWatchListExist(int idUser, string ticker)
        {
            return await _context.watchlists.AnyAsync(e => e.IdUser == idUser && e.ticker == ticker);
        }
    }
}
