using ClashServer.Contracts;
using ClashServer.Entities;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public ProjectController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProject()
        {
            try {
                var projects = await _repository.Project.FindAll().ToListAsync();
                if (projects == null) {
                    return NotFound();
                }
                return Ok(projects);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetAllProject action: {ex.Message}");
                return NotFound(new { message = ex.StackTrace });
            }
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetProjectByName(string name)
        {
            try {
                var project = await _repository.Project.FindByCondition(p => p.Name.Equals(name)).FirstOrDefaultAsync();
                if (project == null) {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetProjectByName action: {ex.Message}");
                return NotFound(new { message = ex.StackTrace });
            }
        }

        [HttpGet("[action]/{projectId}")]
        public async Task<IActionResult> GetProjectById(Guid projectId)
        {
            try {
                var project = await _repository.Project.FindByCondition(p => p.Id == projectId).FirstOrDefaultAsync();
                if (project == null) {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside GetProjectById action: {ex.Message}");
                return NotFound(new { message = ex.StackTrace });
            }
        }

        [HttpGet("[action]/{projectId}")]
        public async Task<IActionResult> GetProjectAndClashesById(Guid projectId)
        {
            try
            {
                var project = await _repository.Project.FindByCondition(p => p.Id == projectId).FirstOrDefaultAsync();
                if (project == null)
                {
                    return NotFound();
                }

                // get clash group
                var clashGroups = await _repository.ClashGroup.FindByCondition(c => c.ProjectId == projectId).ToListAsync();
                if (clashGroups != null)
                {
                    project.ClashGroups = clashGroups;

                    // get clashes
                    foreach (var clashGroup in clashGroups)
                    {
                        var clashes = await _repository.Clash.FindByCondition(c => c.ClashGroupId == clashGroup.Id).ToListAsync();
                        if (clashes != null)
                        {
                            var clashesTemp = new List<Clash>();

                            // get comments & status
                            foreach (var clash in clashes)
                            {
                                var comments = await _repository.Comment.FindByCondition(c => c.ClashId == clash.Id).ToListAsync();
                                if (comments != null)
                                {
                                    comments.Reverse();
                                    clash.Comments = comments;
                                }

                                var states = await _repository.Status.FindByCondition(c => c.ClashId == clash.Id).ToListAsync();
                                if (states != null)
                                {
                                    states.Reverse();
                                    clash.States = states;
                                }

                                clashesTemp.Add(clash);
                            }

                            clashGroup.Clashes = clashesTemp;
                        }
                    }
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProjectById action: {ex.Message}");
                return NotFound(new { message = ex.StackTrace });
            }
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetProjectAndClashesByName(string name)
        {
            try
            {
                var project = await _repository.Project.FindByCondition(p => p.Name.Equals(name)).FirstOrDefaultAsync();
                if (project == null)
                {
                    return NotFound();
                }

                // get clash group
                var clashGroups = await _repository.ClashGroup.FindByCondition(c => c.ProjectId == project.Id).ToListAsync();
                if (clashGroups != null)
                {
                    project.ClashGroups = clashGroups;

                    // get clashes
                    foreach (var clashGroup in clashGroups)
                    {
                        var clashes = await _repository.Clash.FindByCondition(c => c.ClashGroupId == clashGroup.Id).ToListAsync();
                        if (clashes != null)
                        {
                            var clashesTemp = new List<Clash>();

                            // get comments & status
                            foreach (var clash in clashes)
                            {
                                var comments = await _repository.Comment.FindByCondition(c => c.ClashId == clash.Id).ToListAsync();
                                if (comments != null)
                                {
                                    comments.Reverse();
                                    clash.Comments = comments;
                                }

                                var states = await _repository.Status.FindByCondition(c => c.ClashId == clash.Id).ToListAsync();
                                if (states != null)
                                {
                                    states.Reverse();
                                    clash.States = states;
                                }

                                clashesTemp.Add(clash);
                            }

                            clashGroup.Clashes = clashesTemp;
                        }
                    }
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProjectById action: {ex.Message}");
                return NotFound(new { message = ex.StackTrace });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProject([FromBody]Project project)
        {
            try {
                var projectFind = await _repository.Project.FindByCondition(p => p.Name.Equals(project.Name)).FirstOrDefaultAsync();
                if (projectFind != null) {
                    return Content($"This project already existed: {project.Name}");
                }

                project.CreateTime = DateTime.Now;
                _repository.Project.Create(project);
                _repository.Save();

                _logger.LogInfo($"Project created successfully!");

                return Ok(project.Id);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside CreateProject action: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProjectAndClashes([FromBody] Project project)
        {
            try
            {
                var projectFind = await _repository.Project.FindByCondition(p => p.Name.Equals(project.Name)).FirstOrDefaultAsync();
                if (projectFind != null)
                {
                    return BadRequest();
                }

                project.CreateTime = DateTime.Now;
                _repository.Project.Create(project);
                _repository.Save();
                _logger.LogInfo($"Project created successfully!");

                return Ok(project.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateProject action: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> OverrideProject()
        {
            try {
                var oldProject = Request.Form["oldProject"].ToString();
                var project = JsonConvert.DeserializeObject<Project>(Request.Form["newProject"]);
                if (string.IsNullOrEmpty(oldProject) || project == null) {
                    return BadRequest();
                }

                var projectFind = await _repository.Project.FindByCondition(p => p.Name.Equals(oldProject)).FirstOrDefaultAsync();
                if (projectFind == null) {
                    return BadRequest();
                }
                var oldId = projectFind.Id.ToString();
                _repository.Project.Delete(projectFind);
                //project.CreateTime = DateTime.Now;
                //_repository.Project.Create(project);
                //_repository.Save();
                var folderName = "Resources";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName, project.Name);
                var files = Directory.GetFiles(pathToSave).ToList();
                foreach (var file in Directory.GetFiles(pathToSave)) {
                    //var fullPathFile = Path.Combine(pathToSave, file);
                    if (System.IO.File.Exists(file)) {
                        System.IO.File.Delete(file);
                    }
                }
                return Ok(project.Id);
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside OverrideProject action: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProject([FromBody]Project project)
        {
            try {
                var projectFind = await _repository.Project.FindByCondition(p => p.Id.Equals(project.Id)).FirstOrDefaultAsync();
                if (projectFind == null) {
                    return NotFound();
                }
                projectFind.Name = project.Name;
                projectFind.Path = project.Path;
                projectFind.UpdateTime = DateTime.Now;
                _repository.Project.Update(projectFind);
                _repository.Save();
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside UpdateProject action: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpDelete("[action]/{projectName}")]
        public async Task<IActionResult> DeleteProjectByName(string projectName)
        {
            try {
                var projectFind = await _repository.Project.FindByCondition(p => p.Name.Equals(projectName)).FirstOrDefaultAsync();
                if (projectFind == null) {
                    return NotFound();
                }
                _repository.Project.Delete(projectFind);
                _repository.Save();

                // delete the old clash images
                var folderName = "Resources";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName, projectName);
                var files = Directory.GetFiles(pathToSave).ToList();
                foreach (var file in Directory.GetFiles(pathToSave))
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }

                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError($"Something went wrong inside DeleteProjectByName action: {ex.Message}");
                return BadRequest();
            }
        }
    }
}