namespace GuildSimulator
{
    internal class Item
    {
        public string ItemName { get; protected set; }
        public string ItemType { get; protected set; }
        public int QualityRating { get; protected set; }
        public int EleganceRating { get; protected set; }
    }
}