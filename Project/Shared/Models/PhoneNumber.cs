namespace Shared.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public int PersonId {  get; set; }
        public PhoneNumberType Type { get; set; }
        public string Number { get; set; }

    }
    public enum PhoneNumberType
    {
        Home=1,
        Work,
        Mobile
    }
}
