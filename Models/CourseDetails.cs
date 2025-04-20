using BLL_Education.DTO;

namespace EduPlatform.Models
{
    public class CourseDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public int duration { get; set; }
        public double price { get; set; }
        public List<LessonDTO> Lessons { get; set; }
    }
}
