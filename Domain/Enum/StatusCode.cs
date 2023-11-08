namespace ElectronicJournal.Domain.Enum
{
    public enum StatusCode
    {
        //Lesson
        LessonNotFound = 10,
        TimeIsBusy = 11,
        LessonNotCreated = 12,

        //Class
        ClassNotFound = 20,

        OK = 200,
        InternalServerError = 505
    }
}
