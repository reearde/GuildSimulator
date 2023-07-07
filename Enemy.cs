using System;

namespace GuildSimulator
{
    internal class Enemy
    {
        public string Name { get; private set; }
        public string EnemyType { get; private set; }
        public int EnemyRarity { get; private set; }
        public int HealthPoints { get; set; }
        public int ArmorRating { get; private set; }
        public int ArmorCoverage { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public Enemy()
        {
            Random random = new Random();

            EnemyRarity = random.Next(1, 101);
            if (EnemyRarity < 60)
            {
                // Minion Enemy Spawn
                EnemyType = "Minion";
                
            }
            else if (EnemyRarity < 90)
            {
                // Regular Enemy Spawn
                EnemyType = "Regular";
            }
            else
            {
                // Boss Enemy Spawn
                EnemyType = "Boss";
            }
            Name = NameGenerator.GenerateEnemyName(EnemyType);

            HealthPoints = random.Next(EnemyRarity, 101);
            ArmorRating = random.Next(EnemyRarity / 5, 21);
            ArmorCoverage = random.Next(EnemyRarity, 101);
            MinDamage = random.Next(1, EnemyRarity);
            MaxDamage = random.Next(MinDamage, MinDamage + 50);
        }
    }
}