﻿using ElectronicJournal.Domain.Entity;

namespace ElectronicJournal.Domain.ViewModels
{
    public class TeacherViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public IFormFile? Image { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int Role {  get; set; }

        public List<Subject> Subjects { get; set; } = new();
    }
}
