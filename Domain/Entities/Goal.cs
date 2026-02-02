namespace Domain.Entities
{
    public class Goal
    {
        public Guid Id { get; init; }
        public string UserLogin { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreateAt { get; init; }
        public DateTime? Deadline { get; private set; }
#pragma warning disable CS8618 
        public Goal(string userLogin, string title, string? description, DateTime? deadline)
        {
            UserLogin = userLogin;
            SetTitle(title);
            SetDescription(description);
            CreateAt = DateTime.Now;
            SetDeadline(deadline);
        }
        public void SetTitle(string title)
        {
            if(string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("Title can't be empty.");
            Title = title.Trim();
        }
        public void SetDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description)) Description = null;
            Description = description!.Trim();
        }
        public void SetDeadline(DateTime? deadline)
        {
            if (deadline <= CreateAt) throw new ArgumentException("Deadline already fucked up!");
            Deadline = deadline;
        }

        private Goal() { }
#pragma warning restore CS8618 
    }
}
