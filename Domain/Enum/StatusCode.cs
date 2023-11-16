namespace ElectronicJournal.Domain.Enum
{
    public enum StatusCode
    {
        //Lesson
        LessonNotFound = 10,
        TimeIsBusy = 11,
        LessonNotCreated = 12,

        //Class
        ClassNotCreated = 20,
        ClassNotFound = 21,

        //Subject
        SubjectNotCreated = 30,
        SubjectNotFound = 31,

        //Teacher
        TeacherNotCreated = 40,
        TeacherNotFound = 41,

        //Student
        StudentNotCreated = 50,
        StudentNotFound = 51,

        OK = 200,
        InternalServerError = 505
    }
}
