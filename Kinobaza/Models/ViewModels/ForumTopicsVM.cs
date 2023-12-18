namespace Kinobaza.Models.ViewModels
{
    public class ForumTopicsVM
    {
        public string? Login { get; set; }
        public IEnumerable<ForumTopicVM>? Topics { get; set; }
    }
}
