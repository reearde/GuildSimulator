using System;

namespace GuildSimulator
{
    internal class Armor : Item
    {
        public int ArmorRating { get; private set; }
        public int ArmorCoverage { get; private set; }

        public Armor()
        {
            Random random = new Random();

            ItemName = NameGenerator.GenerateArmorName();
            ItemType = "Armor";
            QualityRating = random.Next(0, 101);
            EleganceRating = random.Next(0, 101);
            ArmorRating = random.Next(1, 51);
            ArmorCoverage = random.Next(5, 91);
        }

        public Armor(string name, int quality, int elegance, int armorRating, int armorCoverage)
        {
            ItemName = name;
            ItemType = "Armor";
            QualityRating = quality;
            EleganceRating = elegance;
            ArmorRating = armorRating;
            ArmorCoverage = armorCoverage;
        }
    }
}