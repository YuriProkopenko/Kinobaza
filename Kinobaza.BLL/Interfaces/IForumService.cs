using Kinobaza.BLL.DTO;

namespace Kinobaza.BLL.Interfaces
{
    public interface IForumService
    {
        Task CreateTopic(TopicDTO topicDTO);
        Task UpdateTopic(TopicDTO topicDTO);
        Task DeleteTopic(int id);
        Task DeleteRecord(int id);
        Task<TopicDTO> GetTopicById(int id);
        Task<RecordDTO> GetRecordById(int id);
        Task<IEnumerable<TopicDTO>?> GetAllTopics();
        Task<IEnumerable<RecordDTO>> GetTopicRecords(int topicId);
        Task<string?> GetRecordsContentPaths(int recordId);
    }
}
