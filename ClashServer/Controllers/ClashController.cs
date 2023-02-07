using ClashServer.Contracts;
using ClashServer.Entities;
using ClashServer.Utils;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClashController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public ClashController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("[action]/{clashGroupId}")]
        public async Task<IActionResult> GetClashesByClashGroupId(Guid clashGroupId)
        {
            try {
                var clashes = await _repository.Clash.FindByCondition(c => c.ClashGroupId == clashGroupId).ToListAsync();
                if (clashes != null) {
                    return Ok(clashes);
                }
                return BadRequest("Don't have clashes");
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetClashesByClashGroupId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpGet("[action]/{clashGroupCode}")]
        public async Task<IActionResult> GetClashesByClashGroupCode(string clashGroupCode)
        {
            try
            {
                var clashes = await _repository.Clash.GetClashByClashGroupCode(clashGroupCode);
                if (clashes != null)
                {
                    return Ok(clashes);
                }
                return BadRequest("Don't have clashes");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetClashesByClashGroupId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpGet("[action]/{projectId}")]
        public async Task<IActionResult> GetClashesByProjectId(Guid projectId)
        {
            try {
                var clashGroups = await _repository.ClashGroup.FindByCondition(c => c.ProjectId == projectId).ToListAsync();
                if (clashGroups != null) {
                    var clashes = new List<Clash>();
                    foreach (var clashGroup in clashGroups) {
                        var clashesFind = await _repository.Clash.FindByCondition(c => c.ClashGroupId == clashGroup.Id).ToListAsync();
                        clashes.AddRange(clashesFind);
                    }
                    return Ok(clashes);
                }
                return BadRequest("Don't have clashes");
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetClashesByProjectId action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateClash()
        {
            try {
                var file = Request.Form.Files[0];
                var clash = JsonConvert.DeserializeObject<Clash>(Request.Form["clash"]);
                clash.Comments = null;
                var folderName = Common.FolderName;
                var pathToSave = Common.GetFolderStoreImage();
                if (file.Length > 0) {
                    var fileName = file.FileName;
                    var fullPath = Path.Combine(pathToSave, fileName);// full path save cua file
                    var dbPath = Path.Combine(folderName, fileName);// path save cua file tren app
                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);// copy toi fiupload
                    }
                    clash.ClashImagePath = dbPath;
                }
                else {
                    return BadRequest();
                }

                _repository.Clash.Create(clash);
                await _repository.SaveAsync();
                return Ok(clash.Id);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateClash action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateClashImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                var clash = JsonConvert.DeserializeObject<Clash>(Request.Form["clash"]);
                clash.Comments = null;
                var folderName = Common.FolderName;
                var pathToSave = Common.GetFolderStoreImage();
                if (file.Length > 0)
                {
                    var fileName = file.FileName;
                    var fullPath = Path.Combine(pathToSave, fileName);// full path save cua file
                    var dbPath = Path.Combine(folderName, fileName);// path save cua file tren app
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);// copy toi fiupload
                    }

                    // find the clash in DB
                    var clashFind = await _repository.Clash.FindByCondition(c => c.ClashImagePath == clash.ClashImagePath).FirstOrDefaultAsync();
                    if (clashFind != null)
                    {
                        clashFind.ClashImagePath = dbPath;

                        _repository.Clash.Update(clashFind);
                        await _repository.SaveAsync();
                    }

                }
                else
                {
                    return BadRequest();
                }

                return Ok(clash.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateClash action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateClash()
        {
            try {
                var file = Request.Form.Files[0];
                var clash = JsonConvert.DeserializeObject<Clash>(Request.Form["clash"]);

                if (clash == null || string.IsNullOrEmpty(clash.Name)) {
                    return BadRequest();
                }
                var folderName = Common.FolderName;
                var pathToSave = Common.GetFolderStoreImage();
                var clashFind = await _repository.Clash.FindByCondition(c => c.ClashGroupId == clash.ClashGroupId && c.Name.Equals(clash.Name)).FirstOrDefaultAsync();
                if (clashFind != null) {
                    // delete image of clash
                    var filePath = Path.Combine(pathToSave, clashFind.ClashImagePath);
                    if (System.IO.File.Exists(filePath)) {
                        System.IO.File.Delete(filePath);
                    }
                    if (file.Length > 0) {
                        var fileName = file.FileName;
                        var fullPath = Path.Combine(pathToSave, fileName);// full path save cua file
                        var dbPath = Path.Combine(folderName, fileName);// path save cua file tren app
                        using (var stream = new FileStream(fullPath, FileMode.Create)) {
                            file.CopyTo(stream);// copy toi fiupload
                        }
                        clash.ClashImagePath = dbPath;
                    }
                    else {
                        return BadRequest();
                    }

                    clashFind.Status = clash.Status;
                    clashFind.AssignTo = clash.AssignTo;
                    clashFind.GridLocation = clash.GridLocation;
                    clashFind.Description = clash.Description;
                    clashFind.DateFound = clash.DateFound;
                    clashFind.ClashPoint = clash.ClashPoint;
                    clashFind.Distance = clash.Distance;
                    clashFind.ElementId1 = clash.ElementId1;
                    clashFind.ElementId2 = clash.ElementId2;
                    clashFind.Layer1 = clash.Layer1;
                    clashFind.Layer2 = clash.Layer2;
                    clashFind.ItemName1 = clash.ItemName1;
                    clashFind.ItemName2 = clash.ItemName2;
                    clashFind.ItemType1 = clash.ItemType1;
                    clashFind.ItemType2 = clash.ItemType2;
                    clashFind.ItemPath1 = clash.ItemPath1;
                    clashFind.ItemPath2 = clash.ItemPath2;
                    clashFind.ClashImagePath = clash.ClashImagePath;
                    _repository.Clash.Update(clashFind);
                    await _repository.SaveAsync();
                }
                else {
                    return BadRequest();
                }

                return Ok(clashFind.Id);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside UpdateClash action: {ex.Message}");
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}