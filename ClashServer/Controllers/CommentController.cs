using ClashServer.Contracts;
using ClashServer.Entities;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public CommentController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("[action]/{clashId}")]
        public async Task<IActionResult> GetCommentsByClashId(Guid clashId)
        {
            try {
                var comments = await _repository.Comment.FindByCondition(c => c.ClashId == clashId).ToListAsync();
                if (comments != null) {
                    return Ok(comments);
                }
                return BadRequest();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetCommentsByClashId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateComment([FromBody]Comment comment)
        {
            try {
                var clash = await _repository.Clash.FindByCondition(p => p.Id == comment.ClashId).FirstOrDefaultAsync();
                if (clash == null) {
                    return BadRequest($"No exist clash with Id:{comment.ClashId}");
                }
                _repository.Comment.Create(comment);
                _repository.Save();
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateComment action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}