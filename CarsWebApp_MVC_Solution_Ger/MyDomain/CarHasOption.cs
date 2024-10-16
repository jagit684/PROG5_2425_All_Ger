namespace MyDomain
{
    public class CarHasOption
    {
        public int CarId { get; set; }
        public required Car Car { get; set; }

        public int CarOptionId { get; set; }
        public required CarOption CarOption { get; set; }
    }
}