using System;

namespace GuildSimulator
{
    internal class Adventurer : Occupant
    {
        public int HealthPoints { get; private set; }
        public int ArmorRating { get; private set; }
        public int ArmorCoverage { get; private set; }
        public int Strength { get; private set; }
        public int Wage { get; private set; }
        public Weapon EquippedWeapon { get; private set; }
        public Armor EquippedArmor { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public Adventurer()
        {
            Random random = new Random();

            Name = NameGenerator.GenerateName();
            Occupation = "Adventurer";
            TrainedThisTurn = false;
            HealthPoints = random.Next(0, 100);
            ArmorRating = random.Next(0, 20);
            ArmorCoverage = random.Next(0, 100);
            Strength = random.Next(0, 100);
            Wage = HealthPoints / 20 + Strength / 10 + ArmorCoverage / 20 + ArmorRating / 5;
            EquippedWeapon = null;
            EquippedWeapon = null;
            MinDamage = Strength / 5;
            MaxDamage = Strength / 5;
        }

        public Adventurer(string name, bool trainStatus, int healthPoints, int armorRating, int armorCoverage, int strength, int minDamage, int maxDamage, int wage)
        {
            Name = name;
            Occupation = "Adventurer";
            TrainedThisTurn = trainStatus;
            HealthPoints = healthPoints;
            ArmorRating = armorRating;
            ArmorCoverage = armorCoverage;
            Strength = strength;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Wage = wage;
        }

        public void EquipItem(Item item)
        {
            if (item.ItemType == "Weapon")
            {
                if (EquippedWeapon == null)
                {
                    EquippedWeapon = (Weapon)item;
                    MinDamage += EquippedWeapon.MinDamage;
                    MaxDamage += EquippedWeapon.MaxDamage;
                    Console.WriteLine($"Adventurer {Name} equipped weapon: {EquippedWeapon.ItemName} (Damage: {EquippedWeapon.MinDamage}-{EquippedWeapon.MaxDamage}, " +
                        $"Quality: {EquippedWeapon.QualityRating}, Elegance: {EquippedWeapon.EleganceRating})");
                    Guild.Items.Remove(item);
                }
                else
                {
                    Console.WriteLine($"There is already a weapon equipped: {EquippedWeapon.ItemName} (Damage: {EquippedWeapon.MinDamage}-{EquippedWeapon.MaxDamage}, " +
                        $"Quality: {EquippedWeapon.QualityRating}, Elegance: {EquippedWeapon.EleganceRating})");
                    Console.Write("Would you like to replace it with the current one? (y / n) ");
                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        // Replace
                        UnequipItem(EquippedWeapon.ItemType);
                        EquippedWeapon = (Weapon)item;
                        MinDamage += EquippedWeapon.MinDamage;
                        MaxDamage += EquippedWeapon.MaxDamage;
                        Console.WriteLine($"Adventurer {Name} equipped weapon: {EquippedWeapon.ItemName} (Damage: {EquippedWeapon.MinDamage}-{EquippedWeapon.MaxDamage}, " +
                            $"Quality: {EquippedWeapon.QualityRating}, Elegance: {EquippedWeapon.EleganceRating})");
                        Guild.Items.Remove(item);
                    }
                    else if (input == "n")
                    {
                        // Do nothing
                        Console.WriteLine("The equipped item won't be replaced.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                
            }
            if (item.ItemType == "Armor")
            {
                if (EquippedArmor == null)
                {
                    EquippedArmor = (Armor)item;
                    ArmorCoverage += EquippedArmor.ArmorCoverage;
                    ArmorRating += EquippedArmor.ArmorRating;
                    Console.WriteLine($"Adventurer {Name} equipped armor: {EquippedArmor.ItemName} (Rating: {EquippedArmor.ArmorRating}, Coverage: {EquippedArmor.ArmorCoverage}, " +
                        $"Quality: {EquippedArmor.QualityRating}, Elegance: {EquippedArmor.EleganceRating})");
                    Guild.Items.Remove(item);
                }
                else
                {
                    Console.WriteLine($"There is already an armor equipped: {EquippedArmor.ItemName} (Rating: {EquippedArmor.ArmorRating}, Coverage: {EquippedArmor.ArmorCoverage}, " +
                        $"Quality: {EquippedArmor.QualityRating}, Elegance: {EquippedArmor.EleganceRating})");
                    Console.Write("Would you like to replace it with the current one? (y / n) ");
                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        // Replace
                        UnequipItem(EquippedArmor.ItemType);
                        EquippedArmor = (Armor)item;
                        ArmorCoverage += EquippedArmor.ArmorCoverage;
                        ArmorRating += EquippedArmor.ArmorRating;
                        Console.WriteLine($"Adventurer {Name} equipped armor: {EquippedArmor.ItemName} (Rating: {EquippedArmor.ArmorRating}, Coverage: {EquippedArmor.ArmorCoverage}, " +
                            $"Quality: {EquippedArmor.QualityRating}, Elegance: {EquippedArmor.EleganceRating})");
                        Guild.Items.Remove(item);
                    }
                    else if (input == "n")
                    {
                        // Do nothing
                        Console.WriteLine("The equipped item won't be replaced.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
            }
        }

        public void UnequipItem(string itemType)
        {
            if (itemType == "Weapon")
            {
                Guild.Items.Add(EquippedWeapon);
                Console.WriteLine($"{EquippedWeapon.ItemName} (Damage: {EquippedWeapon.MinDamage}-{EquippedWeapon.MaxDamage}, " +
                                $"Quality: {EquippedWeapon.QualityRating}, Elegance: {EquippedWeapon.EleganceRating}) unequipped.");
                MinDamage -= EquippedWeapon.MinDamage;
                MaxDamage -= EquippedWeapon.MaxDamage;
                EquippedWeapon = null;
            }
            if (itemType == "Armor")
            {
                Guild.Items.Add(EquippedArmor);
                ArmorCoverage -= EquippedArmor.ArmorCoverage;
                ArmorRating -= EquippedArmor.ArmorRating;
                Console.WriteLine($"{EquippedArmor.ItemName} (Rating: {EquippedArmor.ArmorRating}, Coverage: {EquippedArmor.ArmorCoverage}, " +
                            $"Quality: {EquippedArmor.QualityRating}, Elegance: {EquippedArmor.EleganceRating}) unequipped.");
                EquippedArmor = null;
            }
        }

        public bool Battle()
        {
            Random random = new Random();

            // Generate Enemy
            Enemy enemy = new Enemy();

            // Battle with Generated Enemy
            int battleHP = HealthPoints;

            Console.WriteLine($"Adventurer {Name} (HP: {HealthPoints}," +
                                                $" Strength: {Strength}," +
                                                $" Damage: {MinDamage}-{MaxDamage}," +
                                                $" Armor Class: {ArmorRating}," +
                                                $" Armor Coverage {ArmorCoverage})");
            Console.WriteLine($"entered a battle with {enemy.EnemyType} enemy {enemy.Name} (HP: {enemy.HealthPoints}," +
                                                $" Damage: {enemy.MinDamage}-{enemy.MaxDamage}," +
                                                $" Armor Class: {enemy.ArmorRating}," +
                                                $" Armor Coverage {enemy.ArmorCoverage})");

            while (battleHP > 0 || enemy.HealthPoints > 0)
            {
                Console.WriteLine($"Adventurer {Name} HP: {battleHP}/{HealthPoints} - {enemy.EnemyType} Enemy HP: {enemy.HealthPoints}");

                int hitChanceAdventurer = random.Next(0, 101);
                int damage;
                int hitChanceEnemy = random.Next(0, 101);
                int enemyDamage;
                // Adventurer try to hit
                if (hitChanceAdventurer > enemy.ArmorCoverage)
                {
                    // Full Blow
                    damage = random.Next(MinDamage, MaxDamage + 1);

                    Console.WriteLine($"{Name} hits the {enemy.Name} with a full blow. {enemy.Name} takes {damage} damage.");

                    if (damage > 0)
                    {
                        enemy.HealthPoints -= damage;
                    }
                }
                else
                {
                    // Hit to Armor
                    damage = random.Next(MinDamage, MaxDamage + 1) - enemy.ArmorRating;

                    Console.WriteLine($"{Name} hits to the {enemy.Name} armor. {enemy.Name} takes {damage} damage by mitigating a full blow.");

                    if (damage > 0)
                    {
                        enemy.HealthPoints -= damage;
                    }
                }
                // Enemy try to hit
                if (hitChanceEnemy > ArmorCoverage)
                {
                    // Full Blow
                    enemyDamage = random.Next(enemy.MinDamage, enemy.MaxDamage + 1);

                    Console.WriteLine($"{enemy.Name} hits the {Name} with a full blow. {Name} takes {enemyDamage} damage.");

                    if (enemyDamage > 0)
                    {
                        battleHP -= enemyDamage;
                    }
                }
                else
                {
                    // Hit to Armor
                    enemyDamage = random.Next(enemy.MinDamage, enemy.MaxDamage + 1) - ArmorRating;

                    Console.WriteLine($"{enemy.Name} hits to the {Name} armor. {Name} takes {enemyDamage} damage by mitigating a full blow.");

                    if (enemyDamage > 0)
                    {
                        battleHP -= enemyDamage;
                    }
                }
            }
            if (battleHP <= 0)
            {
                // Death
                Console.WriteLine($"{enemy.Name} kills your adventurer. {Name}'s deeds will be remembered.");

                return true;
            }
            else
            {
                // Reward
                Console.WriteLine($"Adventurer {Name} kills the {enemy.EnemyType} enemy {Name}. Enjoy your newly gained fame!");

                Guild.Gold += enemy.EnemyRarity;
                if (enemy.EnemyType == "Regular")
                {
                    Guild.Fame += 1;
                }
                else if (enemy.EnemyType == "Boss")
                {
                    Guild.Fame += 3;
                }

                return false;
            }
        }
    }
}