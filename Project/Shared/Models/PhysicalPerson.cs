namespace Shared.Models
{
    public class PhysicalPerson
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual City? City { get; set; }
        public virtual List<PhoneNumber>? PhoneNumbers { get; set; }
        public string? ImagePath { get; set; }
        public virtual ICollection<Relation> RelatedFrom { get; set; }
        public virtual ICollection<Relation> RelatedTo { get; set; }
    }
    public enum Gender
    {
        Male = 1,
        Female
    }
}
