namespace ElectronicJournal.Domain.Entity
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string ImagePath { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int IdClass { get; set; }
        public Class? Class { get; set; }

        public List<Score> Scores { get; set; } = new();


    }
}
