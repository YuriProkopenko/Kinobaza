namespace Kinobaza.Models.ViewModels
{
    public class ForumVM
    {
        public string? Login { get; set; }
        public IEnumerable<ForumTopicVM>? Topics { get; set; }
    }
}
