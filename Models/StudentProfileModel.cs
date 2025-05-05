namespace EduPlatform.Models
{
    public class StudentProfileModel
    {
        /* Данные студента */
        public string Name { get; set; }
        public string Email { get; set; }

        /* Курсы */
        public string Title { get; set; }
        public int duration { get; set; }
        public double price { get; set; }
    }
}
