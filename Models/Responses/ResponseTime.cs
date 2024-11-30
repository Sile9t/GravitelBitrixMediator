namespace Entities.Responses
{
    public record ResponseTime
    {
        public double Start { get; init; }
        public double Finish { get; init; }
        public double Duration { get; init; }
        public double Processing { get; init; }
        public DateTime DateStart { get; init; }
        public DateTime DateFinish { get; init; }
    }
}
