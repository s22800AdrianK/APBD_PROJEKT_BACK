using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models.DTOs;
using Projekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Configurations
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;
        private readonly IClientService _clientService;

        public StockController(IStockService service, IClientService clientService)
        {
            _service = service;
            _clientService = clientService;
        }

        [HttpGet("company/regex")]
        [Authorize]
        public async Task<IActionResult> GetCompanies(string regex)
        {
            try
            {
                return Ok(await _service.GetCompanies(regex));
            }
            catch (Exception)
            {
                return Ok(await _service.GetCompaniesDB(regex));
            }
        }

        [HttpGet("company")]
        [Authorize]
        public async Task<IActionResult> GetCompany(string ticker)
        {
            if (!await _service.DoesTickerExist(ticker)) return StatusCode(404,"Not found");
          try
            {
                return Ok(await _service.GetCompany(ticker));
          }
          catch
           {
                var r = await _service.GetCompanyDB(ticker);
                if(r!=null)
                return Ok(await _service.GetCompanyDB(ticker));
                else return StatusCode(502, "Coudn't find");
           }
          
        }

        [HttpGet("OHLCs")]
        [Authorize]
        public async Task<IActionResult> GetOHLCs(string ticker, DateTime from, DateTime to, string timespan = "day")
        {
            if (!await _service.DoesTickerExist(ticker)) return StatusCode(404, "Not found");
            if (!_service.isFromSmallerThanTo(from, to)) return StatusCode(400, "wrong dates");
            try
            {
            return Ok(await _service.GetOHLCs(ticker, from, to,timespan));
            }
            catch 
            {
                return Ok(await _service.GetOHLCsDB(ticker, from, to));
            }
        }

        [HttpGet("company/watched")]
        [Authorize(Policy = "CorrectUser")]
        public async Task<IActionResult> GetWatchedCompanies(int idUser)
        {
            if (!await _clientService.DoesClientExists(idUser)) return StatusCode(404, "Not found Client");
            return Ok(await _service.GetWatchedCompanies(idUser));
        }

        [HttpPost("company/watched")]
        [Authorize(Policy = "CorrectUser")]
        public async Task<IActionResult> AddToWatchList(int idUser, string ticker)
        {
            if (!await _service.DoesTickerExistDB(ticker)) return StatusCode(404, "Not found ticker");
            if (!await _clientService.DoesClientExists(idUser)) return StatusCode(404, "Not found Client");
            await _service.AddtoWatchlist(ticker,idUser);
            return Ok();
        }

        [HttpDelete("company/watched")]
        [Authorize(Policy = "CorrectUser")]
        public async Task<IActionResult> DeleteWatchList(int idUser, string ticker)
        {
            if (!await _service.DoesTickerExistDB(ticker)) return StatusCode(404, "Not found ticker");
            if (!await _clientService.DoesClientExists(idUser)) return StatusCode(404, "Not found Client");
            await _service.DeleteWatchlist(ticker, idUser);
            return Ok();
        }
    }
}
