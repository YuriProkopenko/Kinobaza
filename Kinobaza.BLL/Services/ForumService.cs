using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.DAL.Entities;
using Kinobaza.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.BLL.Services
{
    public class ForumService : IForumService
    {
        private readonly IUnitOfWork _uow;

        public ForumService(IUnitOfWork uof) => _uow = uof;

        public async Task CreateTopic(TopicDTO topicDTO)
        {
            var topic = new Topic
            {
                Title = topicDTO.Title,
                Description = topicDTO.Description,
                Author = topicDTO.Author,
                Date = topicDTO.Date
            };
            await _uow.Topics.AddAsync(topic);
            await _uow.Topics.SaveAsync();
        }

        public async Task UpdateTopic(TopicDTO topicDTO)
        {
            //get movie
            var topic = await _uow.Topics.FirstOrDefaultAsync(m => m.Id == topicDTO.Id, includeProperties: "Records") ?? throw new ValidationException("Wrong topic!");

            //get new record
            IEnumerable<Record> records = new List<Record>();
            if(topicDTO.RecordDTOs is not null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<RecordDTO, Record>()
                .ForMember("TopicId", opt => opt.MapFrom(r => r.TopicId)));
                var mapper = new Mapper(config);
                records = mapper.Map<IEnumerable<RecordDTO>, IEnumerable<Record>>(topicDTO.RecordDTOs);
            }

            //update topic
            topic.Title = topicDTO.Title;
            topic.Description = topicDTO.Description;
            topic.Author = topicDTO.Author;
            topic.Date = topicDTO.Date;
            topic.Records = records;
            _uow.Topics.Update(topic);

            //save to db
            await _uow.Topics.SaveAsync();
        }

        public async Task DeleteTopic(int topicId)
        {
            //var topic = await GetTopicById(topicId);
            //if(topic.RecordsIds is not null)
            //{
            //    foreach (var recordId in topic.RecordsIds)
            //    {
            //        await _uow.Records.Delete(recordId);
            //    }
            //}
            //await _uow.Records.SaveAsync();
            await _uow.Topics.Delete(topicId);
            await _uow.Topics.SaveAsync();
        }

        public async Task DeleteRecord(int recordId)
        {
            await _uow.Records.Delete(recordId);
            await _uow.Records.SaveAsync();
        }

        public async Task<IEnumerable<TopicDTO>?> GetAllTopics()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Topic, TopicDTO>()
            .ForMember("RecordDTOs", opt => opt.MapFrom(t => t.Records.Select(r => 
                new RecordDTO { Id = r.Id, Author = r.Author, Date = r.Date, Text = r.Text, ContentPaths = r.ContentPaths}))));
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Topic>, IEnumerable<TopicDTO>>(await _uow.Topics.GetAllAsync());
        }

        public async Task<TopicDTO> GetTopicById(int id)
        {
            var topic = await _uow.Topics.FirstOrDefaultAsync(r => r.Id == id, includeProperties: "Records") ?? throw new ValidationException("Wrong topic!");
            var recordDTOs = new List<RecordDTO>();
            if(topic.Records is not null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Record, RecordDTO>());
                var mapper = new Mapper(config);
                recordDTOs = mapper.Map<IEnumerable<Record>, IEnumerable<RecordDTO>>(topic.Records).ToList();
            }
            return new TopicDTO
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                Author = topic.Author,
                Date = topic.Date,
                RecordDTOs = recordDTOs
            };
        }

        public async Task<RecordDTO> GetRecordById(int id)
        {
            var record = await _uow.Records.FirstOrDefaultAsync(r => r.Id == id) ?? throw new ValidationException("Wrong record!");
            return new RecordDTO
            {
                Id = record.Id,
                Author = record.Author,
                Date = record.Date,
                Text = record.Text,
                TopicId = record.TopicId
            };
        }

        public async Task<IEnumerable<RecordDTO>> GetTopicRecords(int topicId)
        {
            var records = await _uow.Records.GetAllAsync(r => r.TopicId == topicId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Record, RecordDTO>());
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Record>, IEnumerable<RecordDTO>>(records);
        }

        public async Task<string?> GetRecordsContentPaths(int recordId)
        {
            var record = await _uow.Records.FirstOrDefaultAsync(r => r.Id == recordId) ?? throw new ValidationException("Wrong record!");
            return record.ContentPaths;
        }
    }
}
