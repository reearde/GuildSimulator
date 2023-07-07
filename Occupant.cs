namespace GuildSimulator
{
    internal class Occupant
    {
        public string Name { get; protected set; }
        public string Occupation { get; protected set; }
        public bool TrainedThisTurn { get; set; }
    }
}