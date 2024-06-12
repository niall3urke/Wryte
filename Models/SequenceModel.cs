namespace Wryte.Models
{
    public class SequenceModel : INovelItem
    {

        // Properties

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public Guid NovelId { get; set; }

        public Guid Id { get; set; }
        

        public bool Archived { get; set; }

        public string Name { get; set; }

        public int Index { get; set; }

        // Constructors

        public SequenceModel()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Id = Guid.NewGuid();
        }



    }
}
