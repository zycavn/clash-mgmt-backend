using ClashServer.Contracts;
using ClashServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClashServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public SeedController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SeedData()
        {
            try {
                var project = await _repository.Project.FindAll().FirstOrDefaultAsync();
                if (project != null) {
                    return BadRequest();
                }
                project = new Project {
                    Name = "TestProject1",
                    Path = "PathTest",
                    CreateTime = DateTime.Now
                };
                var clashes1 = new List<Clash>() {
                new Clash {
                    Name="TestClash1",

                    Status="Active",
                    AssignTo="MEP",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,

                    ElementId2=1111,
                },
                new Clash {
                    Name="TestClash313",

                    Status="Active",
                    AssignTo="Structure",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,

                    ItemType1="type1",

                    ElementId2=1111,
                    Layer2="layer1",
                    ItemPath2="File > File > CR-BS-S.nwc > GL.VP.VF-INNO-BS-ZZ-M3-S-0001.rvt : 22 : location <Not Shared> > COTE NẮP HẦM > Structural Framing > INNO_S_Be-Rec > 400x600 > INNO_S_Be-Rec > S_MTR_CON-B35",
                },
                };
                var clashes22 = new List<Clash>() {
                    new Clash {
                    Name="TestCla22sh313",

                    Status="Reviewed",
                    AssignTo="Architechture",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,
                    Layer1="layer1",
                    ItemPath1="File > File > CR-BS-E.nwc > GL.VP.VF-INNO-BS-ZZ-M3-E-0001.rvt : 86 : location <Not Shared> > TẦNG B1 > Electrical Equipment > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 2200x1200x600 > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 2200x1200x600 > Solid",
                    ItemName1="name1",
                    ItemType1="type1",

                    ElementId2=1111,
                    Layer2="layer1",
                    ItemPath2="File > File > CR-BS-S.nwc > GL.VP.VF-INNO-BS-ZZ-M3-S-0001.rvt : 22 : location <Not Shared> > TẦNG B1 > Structural Framing > INNO_S_Be-Rec > 800x600 > INNO_S_Be-Rec > S_MTR_CON-B35",
                    ItemName2="name1",
                    ItemType2="type1",
                },
                new Clash {
                    Name="TestCla22sh313",

                    Status="Active",
                    AssignTo="MEP",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,
                    Layer1="layer1",
                    ItemPath1="File > File > CR-BS-E.nwc > GL.VP.VF-INNO-BS-ZZ-M3-E-0001.rvt : 86 : location <Not Shared> > TẦNG B1 > Electrical Equipment > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 1600x800x350 > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 1600x800x350 > Solid",
                    ItemName1="name1",
                    ItemType1="type1",

                    ElementId2=1111,
                    Layer2="layer1",
                    ItemPath2="File > File > CR-BS-S.nwc > GL.VP.VF-INNO-BS-ZZ-M3-S-0001.rvt : 22 : location <Not Shared> > COTE NẮP HẦM > Structural Framing > INNO_S_Be-Rec > 800x1370 > INNO_S_Be-Rec > S_MTR_CON-B35",
                    ItemName2="name1",
                    ItemType2="type1",
                }
                };
                project.ClashGroups = new List<ClashGroup> {
                    new ClashGroup {
                        ClashCode="A1.S3",
                        Clashes=clashes1,
                        Tolerance="Tolesdasd"
                    },
                    new ClashGroup {
                        ClashCode="A1.S4",
                        Clashes=clashes22,
                        Tolerance="Tolesdasd"
                    }
                     };
                _repository.Project.Create(project);

                var project2 = new Project {
                    Name = "TestProject2",
                    Path = "PathTest",
                    CreateTime = DateTime.Now
                };
                var clashes2 = new List<Clash>() {
                new Clash {
                    Name="TestClash2",

                    Status="Active",
                    AssignTo="assignTo",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,
                    Layer1="layer1",

                    ElementId2=1111,

                    Comments=new List<Comment> {
                        new Comment {
                            Description="comment1",
                            Time=DateTime.Now,
                            UserName="MEP"
                        },
                        new Comment {
                            Description="comment2",
                            Time=DateTime.Now,
                            UserName="Architech"
                        }
                    },
                    States=new List<Status> {
                        new Status {
                            NewStatus="Reviewed",
                            UserName="Struct",
                            Time=DateTime.Now
                        },
                        new Status {
                             NewStatus="Active",
                            UserName="MEP",
                            Time=DateTime.Now
                        }
                    }
                },
                new Clash {
                    Name="TestClash3",

                    Status="Active",
                    AssignTo="assignTo",
                    GridLocation="gridLocation",
                    Description="description1",
                    DateFound=DateTime.Now.ToLongDateString(),

                    ClashPoint="clashpoint",

                    ElementId1=1111,
                    Layer1="layer1",
                    ItemPath1="File > File > CR-BS-E.nwc > GL.VP.VF-INNO-BS-ZZ-M3-E-0001.rvt : 86 : location <Not Shared> > TẦNG B1 > Electrical Equipment > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 2200x1200x600 > INNO_E_TỦ ĐIỆN PHÂN PHỐI > TỦ ĐIỆN 2200x1200x600 > Solid",
                    ItemName1="name1",
                    ItemType1="type1",

                    ElementId2=1111,
                    Layer2="layer1",
                    ItemPath2="File > File > CR-BS-S.nwc > GL.VP.VF-INNO-BS-ZZ-M3-S-0001.rvt : 22 : location <Not Shared> > TẦNG B1 > Walls > Basic Wall > S_Con_B35_400 > Basic Wall > S_MTR_CON-B35",
                    ItemName2="name1",
                    ItemType2="type1",

                    Comments=new List<Comment> {
                        new Comment {
                            Description="comment1",
                            Time=DateTime.Now,
                            UserName="MEP"
                        },
                        new Comment {
                            Description="comment2",
                            Time=DateTime.Now,
                            UserName="Architech"
                        }
                    },
                    States=new List<Status> {
                        new Status {
                            NewStatus="Reviewed",
                            UserName="Struct",
                            Time=DateTime.Now
                        },
                        new Status {
                             NewStatus="Active",
                            UserName="MEP",
                            Time=DateTime.Now
                        }
                    }
                }
                };
                project2.ClashGroups = new List<ClashGroup> { new ClashGroup { ClashCode = "M1.A2", Clashes = clashes2, Tolerance = "sssss" } };
                _repository.Project.Create(project2);
                _repository.Save();
                return Ok("123344");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}