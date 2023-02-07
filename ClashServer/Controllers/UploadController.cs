using ClashServer.Entities;
using ClashServer.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("[action]")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public async Task<IActionResult> UploadImages()
        {
            var filePaths = new List<string>();

            var project = JsonConvert.DeserializeObject<Project>(Request.Form["project"]);

            var folderName = Common.FolderName;
            var pathToSave = Path.Combine(Common.GetFolderStoreImage(), project.Name);
            Directory.CreateDirectory(pathToSave);
            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var fileName = file.FileName;
                    var fullPath = Path.Combine(pathToSave, fileName);                        // image path in server
                    var dbPath = Path.Combine(folderName, project.Name, fileName);            // image path in DB

                    filePaths.Add(dbPath);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok(filePaths);
        }
    }
}