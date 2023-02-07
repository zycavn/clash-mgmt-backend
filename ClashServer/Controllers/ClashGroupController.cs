using ClashServer.Contracts;
using ClashServer.Entities;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClashGroupController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public ClashGroupController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("[action]/{projectId}")]
        public async Task<IActionResult> GetClashGroupsByProjectId(Guid projectId)
        {
            try {
                var clashGroups = await _repository.ClashGroup.FindByCondition(c => c.ProjectId == projectId).ToListAsync();
                if (clashGroups != null) {
                    return Ok(clashGroups);
                }
                return BadRequest("Don't have clashGroups");
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetClashGroupsByProjectId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetClashGroupByName([FromBody]dynamic info)
        {
            try {
                var projectId = (Guid)info.ProjectId;
                var clashCode = (string)info.ClashCode;
                var clashGroup = await _repository.ClashGroup.FindByCondition(c => c.ProjectId == projectId && c.ClashCode.Equals(clashCode)).FirstOrDefaultAsync();
                if (clashGroup != null) {
                    return Ok(clashGroup);
                }
                return NotFound("Don't have clashGroup");
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetClashGroupByName action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateClashGroup([FromBody]ClashGroup clashGroup)
        {
            try {
                _repository.ClashGroup.Create(clashGroup);
                await _repository.SaveAsync();

                _logger.LogInfo($"Clash group created successfully: {clashGroup.ClashCode}");

                return Ok(clashGroup.Id);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateClashGroup action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateClashGroups([FromBody] List<ClashGroup> clashGroups)
        {
            try
            {
                foreach (var clashGroup in clashGroups)
                {
                    await CreateClashGroup(clashGroup);
                }

                return NotFound("Don't have clashGroup");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetClashGroupByName action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateClashGroups([FromBody]List<ClashGroup> clashGroups)
        {
            try
            {
                foreach (var clashGroup in clashGroups)
                {
                    // find clashes
                    var oldClashes = await _repository.Clash.GetClashByClashGroupCode(clashGroup.ClashCode);
                    var newClashes = clashGroup.Clashes;

                    var clashesForUpdate = GetClashesForUpdate(oldClashes, newClashes);
                    var clashesForCreate = GetClashesForCreate(oldClashes, newClashes);

                    if (clashesForUpdate.Count > 0)
                        await ProcessUpdateClashesAsync(clashesForUpdate);

                    if (clashesForCreate.Count > 0)
                        await ProcessCreateClashesAsync(clashesForCreate);
                }

                return NotFound("Don't have clashGroup");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetClashGroupByName action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private async Task ProcessCreateClashesAsync(List<Clash> clashesForCreate)
        {
            foreach (var clash in clashesForCreate)
            {
                _repository.Clash.Create(clash);
                await _repository.SaveAsync();
            }    
        }

        private async Task ProcessUpdateClashesAsync(List<Clash> clashesForUpdate)
        {
            foreach (var clash in clashesForUpdate)
            {
                try
                {
                    await _repository.Clash.UpdateClash(clash);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong inside ProcessUpdateClashesAsync action: {ex.Message}");
                }
            }
        }

        private List<Clash> GetClashesForCreate(IEnumerable<Clash> oldClashes, ICollection<Clash> newClashes)
        {
            var exceptClashName = newClashes.Select(c => c.Name).ToList()
                             .Except(oldClashes.Select(c => c.Name).ToList())
                             .ToList();
            var exceptClashes = newClashes.Where(c => exceptClashName.Contains(c.Name)).ToList();

            return exceptClashes;
        }

        private List<Clash> GetClashesForUpdate(IEnumerable<Clash> oldClashes, ICollection<Clash> newClashes)
        {
            var commonClashName = newClashes.Select(c => c.Id).ToList()
                             .Intersect(oldClashes.Select(c => c.Id).ToList())
                             .ToList();
            var commonClashes = newClashes.Where(c => commonClashName.Contains(c.Id)).ToList();

            return commonClashes;
        }
    }
}