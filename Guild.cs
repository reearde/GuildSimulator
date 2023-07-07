using System;
using System.Threading;
using System.Collections.Generic;

namespace GuildSimulator
{
    internal class Guild
    {
        public static string Name { get; set; }
        private static int fame;
        public static int Fame
        {
            get { return fame; }
            set
            {
                fame = value;
                if (fame <= 0)
                {
                    OnGameOver();
                }
                else if (fame >= 100)
                {
                    OnGameVictory();
                }
            }
        }
        private static int gold;
        public static int Gold
        {
            get { return gold; }
            set 
            {
                gold = value;
                if (gold <= 0)
                {
                    OnGameOver();
                }
            }
        }
        public static int Materials { get; set; }
        public static List<Artisan> Artisans { get; set; }
        public static int MaxArtisansAvailable { get; set; }
        public static List<Adventurer> Adventurers { get; set; }
        public static int MaxAdventurersAvailable { get; set; }
        public static List<Item> Items { get; set; }

        public Guild()
        {
            Gold = 100;
            Fame = 5;
            Materials = 0;
            Artisans = new List<Artisan>();
            MaxArtisansAvailable = (Fame / 20) + 3;
            Adventurers = new List<Adventurer>();
            MaxAdventurersAvailable = (Fame / 10) + 4;
            Items = new List<Item>();
        }

        public static event Action GameOverEvent;

        protected static void OnGameOver()
        {
            GameOverEvent?.Invoke();
        }

        public static event Action GameVictoryEvent;

        protected static void OnGameVictory()
        {
            GameVictoryEvent?.Invoke();
        }

        public static void GameOver()
        {
            // Game over logic
            Console.WriteLine("Game over!");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void GameVictory()
        {
            // Game victory logic
            Console.WriteLine("You won the game!");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void HireOccupant()
        {

        }

        public static void FireArtisan(int index)
        {
            Console.WriteLine($"{Artisans[index].Name} fired.");
            Console.WriteLine();
            Fame -= Artisans[index].CraftSkill / 10;
            Artisans.RemoveAt(index);
        }

        public static void FireAdventurer(int index)
        {
            Console.WriteLine($"{Adventurers[index].Name} fired.");
            Console.WriteLine();
            Fame -= Adventurers[index].Wage / 2;
            Adventurers.RemoveAt(index);
        }

        public static void TrainArtisan(int index)
        {
            if (!Artisans[index].TrainedThisTurn)
            {
                Gold -= 10;

                Artisans[index].TrainedThisTurn = true;
                Artisans[index].CraftSkill += 5;
                Artisans[index].Wage = Artisans[index].CraftSkill / 5;

                Console.WriteLine($"{Artisans[index].Name} trained (New Crafting Skill: {Artisans[index].CraftSkill}).");
            }
            else
            {
                Console.WriteLine("This artisan already trained today.");
            }
            Console.WriteLine();
        }

        public static void BuySupplies()
        {
            Gold -= 50;
            Materials += 10;
        }

        public static void ShowcaseItem(int itemIndex)
        {
            // Assess new fame
            if (Items[itemIndex].EleganceRating >= 70)
            {
                Console.WriteLine("The King approves, your artifact will be showcased, you gain fame.");
            }
            else
            {
                Console.WriteLine("The King disapproves, you lose fame.");
            }
            Console.WriteLine();
            Fame += (Items[itemIndex].EleganceRating - 65) / 5;

            // Remove item
            Items.RemoveAt(itemIndex);
        }

        public static void SellItem(int itemIndex)
        {
            // Add earned gold
            Gold += Items[itemIndex].QualityRating / 5;

            Console.WriteLine($"You earned {Items[itemIndex].QualityRating / 5} gold.");
            Console.WriteLine();

            // Remove item
            Items.RemoveAt(itemIndex);
        }

        public static void ListItems()
        {
            Console.WriteLine("Items in the Guild Inventory:");
            int itemCount = 1;
            foreach (Item item in Items)
            {
                Console.Write($"{itemCount}. ");
                if (item.ItemType == "Weapon")
                {
                    Weapon weapon = (Weapon)item;
                    Console.WriteLine($"Type: {weapon.ItemType} - {weapon.ItemName} (Damage: {weapon.MinDamage}-{weapon.MaxDamage}, " +
                                $"Quality: {weapon.QualityRating}, Elegance: {weapon.EleganceRating})");
                }
                if (item.ItemType == "Armor")
                {
                    Armor armor = (Armor)item;
                    Console.WriteLine($"Type: {armor.ItemType} - {armor.ItemName} (Rating: {armor.ArmorRating}, Coverage: {armor.ArmorCoverage}, " +
                            $"Quality: {armor.QualityRating}, Elegance: {armor.EleganceRating})");
                }
                itemCount++;
            }
            Console.WriteLine();
        }

        public static void BattlePhase()
        {
            List<Adventurer> deadAdventurers = new List<Adventurer>();

            foreach (Adventurer adventurer in Adventurers)
            {
                Random random = new Random();

                // Generate Enemy
                Enemy enemy = new Enemy();

                // Battle with Generated Enemy
                int battleHP = adventurer.HealthPoints;

                Console.WriteLine($"Adventurer {adventurer.Name} (HP: {adventurer.HealthPoints}," +
                                                    $" Strength: {adventurer.Strength}," +
                                                    $" Damage: {adventurer.MinDamage}-{adventurer.MaxDamage}," +
                                                    $" Armor Class: {adventurer.ArmorRating}," +
                                                    $" Armor Coverage {adventurer.ArmorCoverage})");
                Console.WriteLine($"entered a battle with {enemy.EnemyType} enemy {enemy.Name} (HP: {enemy.HealthPoints}," +
                                                    $" Damage: {enemy.MinDamage}-{enemy.MaxDamage}," +
                                                    $" Armor Class: {enemy.ArmorRating}," +
                                                    $" Armor Coverage {enemy.ArmorCoverage})");

                while (battleHP > 0 && enemy.HealthPoints > 0)
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine($"Adventurer {adventurer.Name} HP: {battleHP}/{adventurer.HealthPoints} - {enemy.EnemyType} Enemy HP: {enemy.HealthPoints}");

                    int hitChanceAdventurer = random.Next(0, 101);
                    int damage;
                    int hitChanceEnemy = random.Next(0, 101);
                    int enemyDamage;
                    // Adventurer try to hit
                    if (hitChanceAdventurer > enemy.ArmorCoverage)
                    {
                        // Full Blow
                        damage = random.Next(adventurer.MinDamage, adventurer.MaxDamage + 1);
                        if (damage < 0)
                        {
                            damage = 0;
                        }

                        Console.WriteLine($"{adventurer.Name} hits the {enemy.Name} with a full blow. {enemy.Name} takes {damage} damage.");
                        enemy.HealthPoints -= damage;
                    }
                    else
                    {
                        // Hit to Armor
                        damage = random.Next(adventurer.MinDamage, adventurer.MaxDamage + 1) - enemy.ArmorRating;
                        if (damage < 0)
                        {
                            damage = 0;
                        }

                        Console.WriteLine($"{adventurer.Name} hits to the {enemy.Name} armor. {enemy.Name} takes {damage} damage by mitigating a full blow.");
                        enemy.HealthPoints -= damage;
                    }
                    // Enemy try to hit
                    if (hitChanceEnemy > adventurer.ArmorCoverage)
                    {
                        // Full Blow
                        enemyDamage = random.Next(enemy.MinDamage, enemy.MaxDamage + 1);
                        if (enemyDamage < 0)
                        {
                            enemyDamage = 0;
                        }

                        Console.WriteLine($"{enemy.Name} hits the {adventurer.Name} with a full blow. {adventurer.Name} takes {enemyDamage} damage.");
                        battleHP -= enemyDamage;
                    }
                    else
                    {
                        // Hit to Armor
                        enemyDamage = random.Next(enemy.MinDamage, enemy.MaxDamage + 1) - adventurer.ArmorRating;
                        if (enemyDamage < 0)
                        {
                            enemyDamage = 0;
                        }

                        Console.WriteLine($"{enemy.Name} hits to the {adventurer.Name} armor. {adventurer.Name} takes {enemyDamage} damage by mitigating a full blow.");
                        battleHP -= enemyDamage;
                    }
                }

                //Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                if (battleHP <= 0)
                {
                    // Death
                    Console.WriteLine($"{enemy.Name} kills your adventurer. {adventurer.Name}'s deeds will be remembered.");
                    deadAdventurers.Add(adventurer);
                }
                else
                {
                    // Reward
                    Console.Write($"Adventurer {adventurer.Name} kills the {enemy.EnemyType} enemy {enemy.Name}.");

                    Gold += enemy.EnemyRarity;
                    if (enemy.EnemyType == "Regular")
                    {
                        Console.Write("Enjoy your newly gained fame!");
                        Fame += 1;
                    }
                    else if (enemy.EnemyType == "Boss")
                    {
                        Console.Write("Enjoy your newly gained fame!");
                        Fame += 3;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }

            // Remove dead Adventurers from the Guild
            foreach (Adventurer deadAdventurer in deadAdventurers)
            {
                Guild.Adventurers.Remove(deadAdventurer);
            }
        }

        public static void PayWages()
        {
            foreach (Artisan artisan in Artisans)
            {
                Console.WriteLine($"Artisan {artisan.Name} payed for {artisan.Wage} gold.");
                Gold -= artisan.Wage;
            }
            foreach (Adventurer adventurer in Adventurers)
            {
                Console.WriteLine($"Adventurer {adventurer.Name} payed for {adventurer.Wage} gold.");
                Gold -= adventurer.Wage;
            }
        }
    }
}