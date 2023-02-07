using ClashServer.Contracts;
using ClashServer.Entities;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public StatusController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("[action]/{clashId}")]
        public async Task<IActionResult> GetStatesByClashId(Guid clashId)
        {
            try {
                var states = await _repository.Status.FindByCondition(c => c.ClashId == clashId).ToListAsync();
                if (states != null) {
                    return Ok(states);
                }
                return BadRequest();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetStatesByClashId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpGet("[action]/{clashId}")]
        public async Task<IActionResult> GetNewestStatusOfClash(Guid clashId)
        {
            try {
                var states = await _repository.Status.FindByCondition(c => c.ClashId == clashId).ToListAsync();
                if (states != null) {
                    var newestStatus = states.Last();
                    return Ok(newestStatus);
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetNewestStatusOfClash action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStatusChange([FromBody] Status status)
        {
            try {
                var clash = await _repository.Clash.FindByCondition(p => p.Id == status.ClashId).FirstOrDefaultAsync();
                if (clash == null) {
                    return BadRequest($"No exist clash with Id:{status.ClashId}");
                }
                clash.Status = status.NewStatus;
                _repository.Clash.Update(clash);
                _repository.Status.Create(status);
                _repository.Save();
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateStatusChange action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStatus([FromBody] Status status)
        {
            try {
                _repository.Status.Create(status);
                _repository.Save();
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateStatus action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}