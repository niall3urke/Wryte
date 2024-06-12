namespace Wryte.Models
{
    public class SessionModel
    {

        // Properties - foreign keys

        public Guid ChapterId { get; set; }

        public Guid NovelId { get; set; }

        public Guid SceneId { get; set; }

        // Properties

        public int ChapterCount { get; set; }

        public int SceneCount { get; set; }

        public int WordCountStart { get; set; }

        public int WordCountEnd { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public Guid Id { get; set; }

        // Constructors

        public SessionModel()
        {
            Start = DateTime.Now;
            Id = Guid.NewGuid();
        }

    }
}
