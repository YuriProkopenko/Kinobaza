using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.DAL.Entities;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Kinobaza.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumServ;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ForumController(IForumService forumServ, IWebHostEnvironment webHostEnvironment)
        {
            _forumServ = forumServ;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Forum/Topics
        [HttpGet]
        public async Task<IActionResult> Topics()
        {
            try
            {
                //get all topics
                var topics = await _forumServ.GetAllTopics();

                //create new list of topic view models
                IEnumerable<ForumTopicVM> topicVMs = new List<ForumTopicVM>();

                //check if topics exists
                if (topics is not null)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TopicDTO, ForumTopicVM>());
                    var mapper = new Mapper(config);
                    topicVMs = mapper.Map<IEnumerable<TopicDTO>, IEnumerable<ForumTopicVM>>(topics);
                }

                return View(topicVMs);
            }
            catch { return NotFound(); }
        }

        // GET: Forum/Create
        [HttpGet]
        public IActionResult Create()
        {
            //create view model
            var topicVM = new ForumTopicVM();

            return View(topicVM);
        }

        // POST: Forum/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(ForumTopicVM topicVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //create topic DTO
                    var topicDTO = new TopicDTO()
                    {
                        Title = topicVM.Title,
                        Description = topicVM.Description,
                        Author = HttpContext.Session.GetString("login"),
                        Date = DateTime.Now
                    };

                    await _forumServ.CreateTopic(topicDTO);

                    return RedirectToAction(nameof(Topics));
                }
            }
            catch { throw; }

            return View(topicVM);
        }

        // GET: Forum/Delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                //check if id is null
                if (id is null) return NotFound();

                //get a topic dto by id
                var topicDTO = await _forumServ.GetTopicById((int)id);

                //check if topic is null
                if (topicDTO is null) return NotFound();

                //create view model
                ForumTopicVM topicVM = new()
                {
                    Id = topicDTO.Id,
                    Title = topicDTO.Title
                };

                return View(topicVM);
            }
            catch { return NotFound(); }
        }

        // POST: Forum/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                //check if id is null
                if (id is null) return NotFound();

                //check if topic dto is exists
                var topicDTO = await _forumServ.GetTopicById((int)id);
                if (topicDTO is null) return NotFound();

                //remove content of this topic
                var recordDTOs = await _forumServ.GetTopicRecords((int)id);
                if (recordDTOs is not null)
                    foreach (var recordDTO in recordDTOs)
                    {
                        DeleteFiles(_webHostEnvironment, recordDTO.ContentPaths);
                    }

                //remove topic
                await _forumServ.DeleteTopic((int)id);
            }
            catch { throw; }

            return RedirectToAction(nameof(Topics));
        }

        // GET: Forum/Topic/id
        [HttpGet]
        public async Task<IActionResult> Topic(int? id)
        {
            try
            {
                //check if id is null
                if (id is null) return NotFound();

                //get topic DTO by id from db
                var topicDTO = await _forumServ.GetTopicById((int)id);

                //check if topic is null
                if (topicDTO is null) return NotFound();

                //save topic id to temp data
                TempData["TopicId"] = id;

                //get records view models
                var recordDTOs = await _forumServ.GetTopicRecords((int)id);
                IEnumerable<ForumRecordVM> recordVMs = new List<ForumRecordVM>();
                if (recordDTOs is not null)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<RecordDTO, ForumRecordVM>());
                    var mapper = new Mapper(config);
                    recordVMs = mapper.Map<IEnumerable<RecordDTO>, IEnumerable<ForumRecordVM>>(recordDTOs);
                }
                foreach (var recordVM in recordVMs)
                {
                    var contentPathsList = recordVM.ContentPaths?.Split(' ');
                    recordVM.ContentPathsList = contentPathsList;
                }

                //create view model
                ForumTopicVM topicVM = new()
                {
                    Id = topicDTO.Id,
                    Title = topicDTO.Title,
                    Author = topicDTO.Author,
                    Date = topicDTO.Date,
                    Description = topicDTO.Description,
                    RecordVMs = recordVMs
                };

                return View(topicVM);
            }
            catch { return NotFound(); }
        }

        // GET: Forum/RecordCreate
        [HttpGet]
        public IActionResult RecordCreate()
        {
            //create view model
            var recordVM = new ForumRecordVM();

            return View(recordVM);
        }

        // POST: Forum/RecordCreate
        [HttpPost, ActionName("RecordCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordCreateConfirmed(ForumRecordVM recordVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if Tempdata of topic id is not exists
                    if (TempData["TopicId"] is not int topicId)
                        return NotFound();

                    //check if topic is exists
                    var topicDTO = await _forumServ.GetTopicById(topicId);
                    if (topicDTO is null) return NotFound();

                    //apload files if exists
                    IEnumerable<string> aploadedFiles = new List<string>();
                    var files = HttpContext.Request.Form.Files;
                    if (files is not null)
                        aploadedFiles = await UploadFilesAsync(_webHostEnvironment, files);

                    //add files to string
                    var contentPaths = string.Empty;
                    foreach (var file in aploadedFiles)
                    {
                        contentPaths += " " + file;
                    }

                    //create new record DTO
                    var recordDTO = new RecordDTO
                    {
                        TopicId = topicDTO.Id,
                        Author = HttpContext.Session.GetString("login"),
                        Text = recordVM.Text,
                        Date = DateTime.Now,
                        ContentPaths = contentPaths
                    };

                    //add record to topic
                    topicDTO.RecordDTOs?.Add(recordDTO);

                    //save changes to db
                    await _forumServ.UpdateTopic(topicDTO);

                    return RedirectToAction("Topic", new { id = topicId });
                }
                return View(recordVM);
            }
            catch { throw; }
        }

        // POST: Forum/RecordDelete/id
        [HttpPost]
        public async Task<IActionResult> RecordDelete(int? recordId)
        {
            try
            {
                //check if view model is null
                if (recordId is null) return NotFound();

                //remove content
                var contentPaths = await _forumServ.GetRecordsContentPaths((int)recordId);
                DeleteFiles(_webHostEnvironment, contentPaths);

                //delete record
                await _forumServ.DeleteRecord((int)recordId);

                var id = (int?)TempData["TopicId"];
                return RedirectToAction(nameof(Topic), new { Id = id });
            }
            catch { return NotFound(); }    
        }

        private static async Task<IEnumerable<string>> UploadFilesAsync(IWebHostEnvironment whEnv, IEnumerable<IFormFile> files)
        {
            try
            {
                var uploadedFiles = new List<string>();

                //get path
                var webRootPath = whEnv.WebRootPath;
                var upload = webRootPath + @"\users\content\";

                //possible extensions
                var imageExtensions = ".jpg.png.jpeg.webp.avif";
                var audioExtensions = ".mp3.ogg";
                var videoExtensions = ".mp4.ogv.webm";

                //create content variables
                var extension = string.Empty;
                var pre = string.Empty;
                var fileName = string.Empty;
                var filePath = string.Empty;
                foreach (var file in files)
                {
                    //get extension
                    extension = Path.GetExtension(file.FileName);

                    //get prefix
                    if (imageExtensions.Contains(extension)) pre = "image";
                    if (audioExtensions.Contains(extension)) pre = "audio";
                    if (videoExtensions.Contains(extension)) pre = "video";

                    //get new file name
                    fileName = pre + Guid.NewGuid().ToString() + extension;

                    //content file path
                    string contentFilePath = @"\users\content\" + fileName;

                    //save new file to db
                    filePath = Path.Combine(upload, fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    //add file to list
                    uploadedFiles.Add(contentFilePath);
                }

                return uploadedFiles;
            }
            catch { throw; }
        }

        private static void DeleteFiles(IWebHostEnvironment whEnv, string? contentPaths)
        {
            if (contentPaths is not null)
            {
                var filePath = string.Empty;
                var contentPathsList = contentPaths.Split(" ");
                foreach (var contentPath in contentPathsList)
                {
                    if (contentPath.Length > 0)
                    {
                        filePath = whEnv.WebRootPath + contentPath;
                        if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    }
                }
            }
        }
    }
}