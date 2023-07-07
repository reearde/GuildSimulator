using System;

namespace GuildSimulator
{
    internal class Weapon : Item
    {
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public Weapon()
        {
            Random random = new Random();

            ItemName = NameGenerator.GenerateWeaponName();
            ItemType = "Weapon";
            QualityRating = random.Next(0,101);
            EleganceRating = random.Next(0, 101);
            MinDamage = random.Next(1, 50);
            MaxDamage = random.Next(MinDamage, MinDamage + 50);
        }

        public Weapon(string name, int quality, int elegance, int minDamage, int maxDamage)
        {
            ItemName = name;
            ItemType = "Weapon";
            QualityRating = quality;
            EleganceRating = elegance;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}