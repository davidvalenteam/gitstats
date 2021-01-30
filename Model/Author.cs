namespace GitStats.Model
{
    internal class Author
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public long LinesAdded { get; set; }
        public long LinesDeleted { get; set; }
        public long TotalCommits { get; set; }

        public Author(string name, string email)
        {
            Name = name;
            Email = email;
        }

        private Author() { }
    }
}


