using System;

namespace GuildSimulator
{
    internal class Artisan : Occupant
    {
        public int CraftSkill { get; set; }
        public int Wage { get; set; }

        public Item ProduceItem()
        {
            Item producedItem;
            Random random = new Random();

            if (random.Next(0,2) == 0)
            {
                // Produce Weapon
                Weapon weapon = new Weapon();
                producedItem = weapon;

                Console.WriteLine($"Artisan {Name} produced a weapon: {weapon.ItemName} (Damage: {weapon.MinDamage}-{weapon.MaxDamage}, " +
                    $"Quality: {weapon.QualityRating}, Elegance: {weapon.EleganceRating})");

                return producedItem;
            }
            else
            {
                // Produce Armor
                Armor armor = new Armor();
                producedItem = armor;

                Console.WriteLine($"Artisan {Name} produced an armor: {armor.ItemName} (Rating: {armor.ArmorRating}, Coverage: {armor.ArmorCoverage}, " +
                    $"Quality: {armor.QualityRating}, Elegance: {armor.EleganceRating})");

                return producedItem;
            }
        }

        public Artisan()
        {
            Random random = new Random();

            Name = NameGenerator.GenerateName();
            Occupation = "Artisan";
            TrainedThisTurn = false;
            CraftSkill = random.Next(0, 100);
            Wage = CraftSkill / 5;
        }

        public Artisan(string name, bool trainStatus, int craftSkill, int wage)
        {
            Name = name;
            Occupation = "Artisan";
            TrainedThisTurn = trainStatus;
            CraftSkill = craftSkill;
            Wage = wage;
        }
    }
}