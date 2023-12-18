namespace Kinobaza.Models.ViewModels
{
    public class UserAccountsVM
    {
        public IEnumerable<UserAccountVM>? UserAccountVMs { get; set; }
        public string? SearchLogin { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
    }
}
