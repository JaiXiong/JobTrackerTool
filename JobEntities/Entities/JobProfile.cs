namespace JobEntities.Entities
{
    public class JobProfile
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }

        public required string Employer { get; set; }

        public required string WorkAction { get; set; }

        public string? ContactTitle { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactMethod { get; set; }
        public string? Result { get; set; }
    }

}
