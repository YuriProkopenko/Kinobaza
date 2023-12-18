using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace Kinobaza.Controllers
{
    public class ForumController : Controller
    {
        private readonly ITopicRepository _topicRepo;
        private readonly IRecordRepository _recordRepo;

        public ForumController(ITopicRepository topicRepo, IRecordRepository recordRepo)
        {
            _topicRepo = topicRepo;
            _recordRepo = recordRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Topics()
        {
            //get all topics
            var topics = await _topicRepo.GetAllAsync(includeProperties: "Records");

            //create new list of topic view models
            List<ForumTopicVM> topicVM = new();

            //check if topics exists fill list of view model
            if (topics is not null)
            {
                foreach (var topic in topics)
                {
                    //fill topicsVM list
                    topicVM.Add(new ForumTopicVM()
                    {
                        Id = topic.Id,
                        Login = HttpContext.Session.GetString("login"),
                        Title = topic.Title,
                        Description = topic.Description,
                        Author = topic.Author,
                        Date = topic.Date,
                        RecordsCount = topic.Records?.Count
                    });
                }
            }

            //create view model
            var topicsVM = new ForumTopicsVM()
            {
                Login = HttpContext.Session.GetString("login"),
                Topics = topicVM
            };

            return View(topicsVM);
        }

        [HttpGet]
        public IActionResult TopicCreate()
        {
            return View();
        }

        [HttpPost, ActionName("TopicCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopicCreateConfirmed(ForumTopicVM? topicVM)
        {
            //cxheck if viw model is null
            if(topicVM is null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //create new topic
                    var topic = new Topic()
                    {
                        Title = topicVM.Title,
                        Description = topicVM.Description,
                        Author = HttpContext.Session.GetString("login"),
                        Date = DateTime.Now,
                        Records = new List<Record>()
                    };

                    await _topicRepo.AddAsync(topic);
                    await _topicRepo.SaveAsync();

                    return RedirectToAction(nameof(Topics));
                }
                catch { throw; }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TopicDelete(int? id)
        {
            //check if id is null
            if(id is null) return NotFound();

            //get topic by id
            var topic = await _topicRepo.FirstOrDefaultAsync(t => t.Id == id);

            //check if topic is null
            if(topic is null) return NotFound();

            //create view model
            var topicVM = new ForumTopicVM() { Title = topic.Title };

            return View(topicVM);
        }

        [HttpPost, ActionName("TopicDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopicDeleteConfirmed(ForumTopicVM? topicVM)
        {
            try
            {
                //check if id or topic is null
                if (topicVM is null || await _topicRepo.GetAllAsync() is null) return NotFound();
                var topic = await _topicRepo.FirstOrDefaultAsync(t => t.Id == topicVM.Id, includeProperties: "Records");
                if (topic is null) return NotFound();

                //remove records and topics
                _topicRepo.Remove(topic);
                await _topicRepo.SaveAsync();
            }
            catch { throw; }

            return RedirectToAction(nameof(Topics));
        }

        [HttpGet]
        public async Task<IActionResult> Topic(int? id) 
        { 
            //check if id is null
            if (id is null) return NotFound();

            //get topic by id from db
            var topic = await _topicRepo.FirstOrDefaultAsync(t => t.Id == id, includeProperties: "Records");

            //check if topic is null
            if (topic is null) return NotFound();

            TempData["TopicId"] = id;

            //get recordsVM
            var recordsVM = new List<ForumRecordVM>();
            if (topic.Records is not null)
            {
                foreach (var record in topic.Records)
                {
                    recordsVM.Add(new ForumRecordVM()
                    {
                        Id = record.Id,
                        Author = record.Author,
                        Text = record.Text,
                        Date = record.Date,
                    }); ;
                }
            }

            //create view model
            var topicVM = new ForumTopicVM()
            {
                Id = topic.Id,
                Login = HttpContext.Session.GetString("login"),
                Title = topic.Title,
                Author = topic.Author,
                Date = topic.Date,
                Description = topic.Description,
                RecordsVM = recordsVM
            };

            return View(topicVM);
        }

        [HttpGet]
        public IActionResult RecordCreate() 
        { 
            return View();
        }

        [HttpPost, ActionName("RecordCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordCreateConfirmed(ForumRecordVM? recordVM)
        {
            //check if topic view model is null
            if (recordVM is null) return NotFound();

            TempData["TopicId"] = recordVM.TopicId;

            if (ModelState.IsValid)
            {
                try
                {
                    //get topic by id
                    var topic = await _topicRepo.FirstOrDefaultAsync(t => t.Id == recordVM.TopicId);

                    //check if topic is null
                    if (topic is null) return NotFound();

                    //create new record
                    var record = new Record
                    {
                        Author = HttpContext.Session.GetString("login"),
                        Text = recordVM.Text,
                        Date = DateTime.Now,
                        Topic = topic
                    };

                    //add record to topic
                    topic.Records?.Add(record);

                    //save changes to db
                    _topicRepo.Update(topic);
                    await _topicRepo.SaveAsync();

                    return RedirectToAction("Topic", new { id = recordVM.TopicId });
                }
                catch { throw; }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecordDelete(int? recordId)
        {
            //check if view model is null
            if (recordId is null) return NotFound();

            //get topic
            var record = await _recordRepo.FirstOrDefaultAsync(r => r.Id == recordId);

            //check if record is not exists
            if (record is null) return NotFound();

            //remove record and save to db
            _recordRepo.Remove(record);
            await _recordRepo.SaveAsync();

            int? topicId = (int)TempData["TopicId"];

            return RedirectToAction(nameof(Topic), new { Id = topicId });
        }
    }

}