namespace GuestLogix.Model
{
    public class Airline
    {
        public virtual string Name { get; set; }

        public virtual string TwoDigitCode { get; set; }

        public virtual string ThreeDigitCode { get; set; }

        public virtual string Country { get; set; }
    }
}
