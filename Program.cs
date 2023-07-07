using System;
using System.Collections.Generic;
using System.IO;
using ConstelLite;

namespace GuildSimulator
{
    internal class Program
    {
        static readonly GraphEngine MyEngine = new GraphEngine();

        public static void SaveGame(GameState savedGameState)
        {
            // DELETE previous contents on the same game file
            MyEngine.ExecuteQuery("MATCH (n) DELETE n");

            // CREATE Game State node
            MyEngine.ExecuteQuery($"CREATE (s:State {{gameState: '{savedGameState}'}})");

            string saveQuery;
            // CREATE Guild node
            saveQuery = $"CREATE (g:Guild {{name: '{Guild.Name}', fame: {Guild.Fame}, gold: {Guild.Gold}, materials: {Guild.Materials}, maxArtisansAvailable: {Guild.MaxArtisansAvailable}, maxAdventurersAvailable: {Guild.MaxAdventurersAvailable}}})";

            // CREATE Occupant nodes and their relationships with the Guild.
            // CREATE Artisan nodes
            for (int i = 0; i < Guild.Artisans.Count; i++)
            {
                saveQuery = saveQuery + $", (art{i}:Occupant {{name: '{Guild.Artisans[i].Name}', occupation: {Guild.Artisans[i].Occupation}, craftSkill: {Guild.Artisans[i].CraftSkill}, wage: {Guild.Artisans[i].Wage}, trainStatus: '{Guild.Artisans[i].TrainedThisTurn}'}})-[r:WORKS_FOR]->(g)";
            }
            // CREATE Adventurer nodes
            for (int i = 0; i < Guild.Adventurers.Count; i++)
            {
                saveQuery = saveQuery + $", (adv{i}:Occupant {{name: '{Guild.Adventurers[i].Name}', occupation: {Guild.Adventurers[i].Occupation}," +
                    $" HP: {Guild.Adventurers[i].HealthPoints}, wage: {Guild.Adventurers[i].Wage}, trainStatus: '{Guild.Adventurers[i].TrainedThisTurn}'," +
                    $" armorRating: {Guild.Adventurers[i].ArmorRating}, armorCoverage: {Guild.Adventurers[i].ArmorCoverage}, strength: {Guild.Adventurers[i].Strength}," +
                    $" minDamage: {Guild.Adventurers[i].MinDamage}, maxDamage: {Guild.Adventurers[i].MaxDamage}}})-[r:WORKS_FOR]->(g)";
                // CREATE Item nodes and their relationships with the Adventurers
                if (Guild.Adventurers[i].EquippedWeapon != null)
                {
                    saveQuery = saveQuery + $", (ew{i}:Item {{name: '{Guild.Adventurers[i].EquippedWeapon.ItemName}', " +
                        $"type: '{Guild.Adventurers[i].EquippedWeapon.ItemType}', " +
                        $"quality: {Guild.Adventurers[i].EquippedWeapon.QualityRating}, " +
                        $"elegance: {Guild.Adventurers[i].EquippedWeapon.EleganceRating}, " +
                        $"minDamage: {Guild.Adventurers[i].EquippedWeapon.MinDamage}, maxDamage: {Guild.Adventurers[i].EquippedWeapon.MaxDamage}}})-[:EQUIPPED_BY]->(adv{i})";
                }
                if (Guild.Adventurers[i].EquippedArmor != null)
                {
                    saveQuery = saveQuery + $", (ea{i}:Item {{name: '{Guild.Adventurers[i].EquippedArmor.ItemName}', " +
                        $"type: '{Guild.Adventurers[i].EquippedArmor.ItemType}', " +
                        $"quality: {Guild.Adventurers[i].EquippedArmor.QualityRating}, " +
                        $"elegance: {Guild.Adventurers[i].EquippedArmor.EleganceRating}, " +
                        $"armorRating: {Guild.Adventurers[i].EquippedArmor.ArmorRating}, armorCoverage: {Guild.Adventurers[i].EquippedArmor.ArmorCoverage}}})-[:EQUIPPED_BY]->(adv{i})";
                }
            }
            // CREATE Item nodes for items in the Guild inventory
            for (int i = 0; i < Guild.Items.Count; i++)
            {
                if (Guild.Items[i].ItemType == "Weapon")
                {
                    Weapon invetoryWeapon = (Weapon)Guild.Items[i];
                    saveQuery = saveQuery + $", (iw{i}:Item {{name: '{invetoryWeapon.ItemName}', " +
                        $"type: '{invetoryWeapon.ItemType}', " +
                        $"quality: {invetoryWeapon.QualityRating}, " +
                        $"elegance: {invetoryWeapon.EleganceRating}, " +
                        $"minDamage: {invetoryWeapon.MinDamage}, maxDamage: {invetoryWeapon.MaxDamage}}})-[:RESIDES_IN]->(g)";
                }
                else if (Guild.Items[i].ItemType == "Armor")
                {
                    Armor inventoryArmor = (Armor)Guild.Items[i];
                    saveQuery = saveQuery + $", (ia{i}:Item {{name: '{inventoryArmor.ItemName}', " +
                        $"type: '{inventoryArmor.ItemType}', " +
                        $"quality: {inventoryArmor.QualityRating}, " +
                        $"elegance: {inventoryArmor.EleganceRating}, " +
                        $"armorRating: {inventoryArmor.ArmorRating}, armorCoverage: {inventoryArmor.ArmorCoverage}}})-[:RESIDES_IN]->(g)";
                }
            }
            // Execute Save Query
            MyEngine.ExecuteQuery(saveQuery);
            // Print all Nodes and Relationships for Debug
            DebugPrintDatabaseNodes();
            DebugPrintDatabaseRelationships();
            // Serialize to File
            Console.Write("Enter the file name to be saved: ");
            string inputBuffer = Console.ReadLine();
            MyEngine.SerializeGraphToFile(inputBuffer);
            Console.WriteLine($"Game saved to {inputBuffer}");
        }

        public static void LoadGame()
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] dbFiles = Directory.GetFiles(directoryPath, "*.db");

            if (dbFiles.Length > 0)
            {
                Console.WriteLine("These save files found:");

                foreach (string filePath in dbFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    Console.WriteLine(fileName);
                }
                Console.Write("Enter the save file name to be loaded: ");
                string inputBuffer = Console.ReadLine();

                try
                {
                    if (File.Exists(inputBuffer + ".db"))
                    {
                        // File exists
                        MyEngine.DeserializeGraphFromFile(inputBuffer);
                        Console.WriteLine($"Saved game {inputBuffer} loaded.");
                    }
                    else
                    {
                        // File does not exist, handle the error
                        Console.WriteLine($"The save file '{inputBuffer}' does not exist.");
                    }
                }
                catch (FileNotFoundException ex)
                {
                    // Handle the exception
                    Console.WriteLine("An error occurred while accessing the file:");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("There are no save files found.");
            }

            // Debug Print All Database Nodes
            DebugPrintDatabaseNodes();

            // Set Loaded Game State
            string loadedGameState = MyEngine.ExecuteQuery("MATCH (s) WHERE s:State RETURN s.gameState").Replace("'", "");

            // Set Guild Attributes
            string loadedGuildName = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.name").Replace("'", "");
            string loadedGuildFame = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.fame");
            string loadedGuildGold = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.gold");
            string loadedGuildMaterials = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.materials");
            string loadedGuildMaxArtisansAvailable = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.maxArtisansAvailable");
            string loadedGuildMaxAdventurersAvailable = MyEngine.ExecuteQuery("MATCH (g) WHERE g:Guild RETURN g.maxAdventurersAvailable");

            // ### Configuration
            // ## Guild Configuration
            Guild.Name = loadedGuildName;
            Guild.Fame = Convert.ToInt32(loadedGuildFame);
            Guild.Gold = Convert.ToInt32(loadedGuildGold);
            Guild.Materials = Convert.ToInt32(loadedGuildMaterials);
            Guild.MaxArtisansAvailable = Convert.ToInt32(loadedGuildMaxArtisansAvailable);
            Guild.MaxAdventurersAvailable = Convert.ToInt32(loadedGuildMaxAdventurersAvailable);

            // ## Artisan Configuration
            MyEngine.ExecuteQuery("MATCH (n {occupation: Artisan}) RETURN n");
            HashSet<Node> artisanNodes = GraphEngine.matchResult.Item1;
            if (Guild.Artisans != null)
            {
                Guild.Artisans.Clear();
            }
            
            foreach (Node artisanNode in artisanNodes)
            {
                string loadedArtisanName = artisanNode.Properties["name"].Replace("'", "");
                bool loadedArtisanTrainStatus = false;
                if (artisanNode.Properties["trainStatus"].Replace("'", "") == "True")
                {
                    loadedArtisanTrainStatus = true;
                }
                else if (artisanNode.Properties["trainStatus"].Replace("'", "") == "False")
                {
                    loadedArtisanTrainStatus = false;
                }
                string loadedArtisanCraftSkill = artisanNode.Properties["craftSkill"];
                string loadedArtisanWage = artisanNode.Properties["wage"];

                Guild.Artisans.Add(new Artisan(loadedArtisanName, loadedArtisanTrainStatus, Convert.ToInt32(loadedArtisanCraftSkill), Convert.ToInt32(loadedArtisanWage)));
                Console.WriteLine(Guild.Artisans.Count);
            }
            // ## Adventurer Configuration
            MyEngine.ExecuteQuery("MATCH (n {occupation: Adventurer}) RETURN n");
            HashSet<Node> adventurerNodes = GraphEngine.matchResult.Item1;
            if (Guild.Artisans != null)
            {
                Guild.Adventurers.Clear();
            }

            foreach (Node adventurerNode in adventurerNodes)
            {
                string loadedAdventurerName = adventurerNode.Properties["name"].Replace("'", "");
                bool loadedAdventurerTrainStatus = false;
                if (adventurerNode.Properties["trainStatus"].Replace("'", "") == "True")
                {
                    loadedAdventurerTrainStatus = true;
                }
                else if (adventurerNode.Properties["trainStatus"].Replace("'", "") == "False")
                {
                    loadedAdventurerTrainStatus = false;
                }
                string loadedAdventurerHP = adventurerNode.Properties["HP"];
                string loadedAdventurerArmorRating = adventurerNode.Properties["armorRating"];
                string loadedAdventurerArmorCoverage = adventurerNode.Properties["armorCoverage"];
                string loadedAdventurerStrength = adventurerNode.Properties["strength"];
                string loadedAdventurerMinDamage = adventurerNode.Properties["minDamage"];
                string loadedAdventurerMaxDamage = adventurerNode.Properties["maxDamage"];
                string loadedAdventurerWage = adventurerNode.Properties["wage"];

                Guild.Adventurers.Add(new Adventurer(loadedAdventurerName, loadedAdventurerTrainStatus, Convert.ToInt32(loadedAdventurerHP), Convert.ToInt32(loadedAdventurerArmorRating),
                    Convert.ToInt32(loadedAdventurerArmorCoverage), Convert.ToInt32(loadedAdventurerStrength), Convert.ToInt32(loadedAdventurerMinDamage), Convert.ToInt32(loadedAdventurerMaxDamage), Convert.ToInt32(loadedAdventurerWage)));
                Console.WriteLine(Guild.Adventurers.Count);
            }

            // ## Equipped Item Configuration + Inventory Item Configuration
            if (Guild.Items != null)
            {
                Guild.Items.Clear();
            }

            MyEngine.ExecuteQuery("MATCH ()-[r:EQUIPPED_BY]->() RETURN r"); // Şimdilik Hatalı, Matches all relationships instead
            HashSet<Relationship> itemRelationships = GraphEngine.matchResult.Item2;
            foreach (Relationship itemRelationship in itemRelationships)
            {
                
                if (itemRelationship.RelationshipType == ":EQUIPPED_BY")
                {
                    string loadedItemName = itemRelationship.SourceNode.Properties["name"].Replace("'", "");
                    string loadedItemType = itemRelationship.SourceNode.Properties["type"].Replace("'", "");
                    string loadedItemQuality = itemRelationship.SourceNode.Properties["quality"];
                    string loadedItemElegance = itemRelationship.SourceNode.Properties["elegance"];
                    if (loadedItemType == "Weapon")
                    {
                        string loadedItemMinDamage = itemRelationship.SourceNode.Properties["minDamage"];
                        string loadedItemMaxDamage = itemRelationship.SourceNode.Properties["maxDamage"];
                        foreach (Adventurer adventurer in Guild.Adventurers)
                        {
                            if (adventurer.Name == itemRelationship.TargetNode.Properties["name"].Replace("'", "")) // If adventurer name match, give them the Weapon
                            {
                                Weapon weapon = new Weapon(loadedItemName, Convert.ToInt32(loadedItemQuality), Convert.ToInt32(loadedItemElegance), Convert.ToInt32(loadedItemMinDamage), Convert.ToInt32(loadedItemMaxDamage));
                                adventurer.EquipItem(weapon);
                            }
                        }
                    }
                    else if (loadedItemType == "Armor")
                    {
                        string loadedItemArmorRating = itemRelationship.SourceNode.Properties["armorRating"];
                        string loadedItemArmorCoverage = itemRelationship.SourceNode.Properties["armorCoverage"];
                        foreach (Adventurer adventurer in Guild.Adventurers)
                        {
                            if (adventurer.Name == itemRelationship.TargetNode.Properties["name"].Replace("'", "")) // If adventurer name match, give them the Armor
                            {
                                Armor armor = new Armor(loadedItemName, Convert.ToInt32(loadedItemQuality), Convert.ToInt32(loadedItemElegance), Convert.ToInt32(loadedItemArmorRating), Convert.ToInt32(loadedItemArmorCoverage));
                                adventurer.EquipItem(armor);
                            }
                        }
                    }
                }
                else if (itemRelationship.RelationshipType == ":RESIDES_IN")
                {
                    string loadedItemName = itemRelationship.SourceNode.Properties["name"].Replace("'", "");
                    string loadedItemType = itemRelationship.SourceNode.Properties["type"].Replace("'", "");
                    string loadedItemQuality = itemRelationship.SourceNode.Properties["quality"];
                    string loadedItemElegance = itemRelationship.SourceNode.Properties["elegance"];

                    if (loadedItemType == "Weapon")
                    {
                        string loadedItemMinDamage = itemRelationship.SourceNode.Properties["minDamage"];
                        string loadedItemMaxDamage = itemRelationship.SourceNode.Properties["maxDamage"];
                        Guild.Items.Add(new Weapon(loadedItemName, Convert.ToInt32(loadedItemQuality), Convert.ToInt32(loadedItemElegance), Convert.ToInt32(loadedItemMinDamage), Convert.ToInt32(loadedItemMaxDamage)));
                    }
                    else if (loadedItemType == "Armor")
                    {
                        string loadedItemArmorRating = itemRelationship.SourceNode.Properties["armorRating"];
                        string loadedItemArmorCoverage = itemRelationship.SourceNode.Properties["armorCoverage"];
                        Guild.Items.Add(new Armor(loadedItemName, Convert.ToInt32(loadedItemQuality), Convert.ToInt32(loadedItemElegance), Convert.ToInt32(loadedItemArmorRating), Convert.ToInt32(loadedItemArmorCoverage)));
                    }
                }
            }

            // ## Game State Configuration
            if (loadedGameState == "GuildCreation")
            {
                gameState = GameState.GuildCreation;
            }
            else if (loadedGameState == "GuildManagement")
            {
                gameState = GameState.GuildManagement;
            }
            else if (loadedGameState == "ItemProduction")
            {
                gameState = GameState.ItemProduction;
            }
            else if (loadedGameState == "AdventurePreparation")
            {
                gameState = GameState.AdventurePreparation;
            }
            else if (loadedGameState == "Adventuring")
            {
                gameState = GameState.Adventuring;
            }
        }

        public static void DebugPrintDatabaseNodes()
        {
            // See All nodes in the database
            MyEngine.ExecuteQuery("MATCH (n) RETURN n");
        }

        public static void DebugPrintDatabaseRelationships()
        {
            // See All relatioships in the database
            MyEngine.ExecuteQuery("MATCH ()-[r]->() RETURN r");
        }

        // States of the Game
        public enum GameState { GuildCreation, GuildManagement, ItemProduction , AdventurePreparation, Adventuring }
        public static GameState gameState;

        static void Main(string[] args)
        {
            Guild.GameOverEvent += Guild.GameOver; // Subscribe to the GameOverEvent
            Guild.GameVictoryEvent += Guild.GameVictory; // Subscribe to the GameVictoryEvent
            Guild guild = new Guild();

            bool gameIsOpened = false;

            while (true)
            {
                Console.WriteLine("Welcome to Guild Simulator!");
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Load Game");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                Console.Clear();

                while (!gameIsOpened)
                {
                    switch (input)
                    {
                        case "1": // New Game
                            gameState = GameState.GuildCreation;
                            gameIsOpened = true;
                            break;
                        case "2": // Load Game
                            Program.LoadGame();
                            gameIsOpened = true;
                            break;
                        case "3": // Exit
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }

                while (gameIsOpened)
                {
                    if (gameState == GameState.GuildCreation)
                    {
                        while (Guild.Name == null || Guild.Name == "")
                        {
                            Console.Write("Enter your guild's name: ");
                            Guild.Name = Console.ReadLine().Replace("'", "");
                            Console.Clear();
                        }
                        gameState = GameState.GuildManagement;
                    }
                    else if (gameState == GameState.GuildManagement)
                    {
                        Console.WriteLine("I----------GUILD MANAGEMENT PHASE----------I");
                        Console.WriteLine($"I---{Guild.Name} - Fame: {Guild.Fame}, Gold: {Guild.Gold}, Materials: {Guild.Materials}---I");
                        Console.WriteLine("I------------------------------------------I");
                        Console.WriteLine("1. Hire Artisan");
                        Console.WriteLine("2. Train Artisan");
                        Console.WriteLine("3. Fire Artisan");
                        Console.WriteLine("4. Hire Adventurer");
                        Console.WriteLine("5. Train Adventurer");
                        Console.WriteLine("6. Fire Adventurer");
                        Console.WriteLine("7. Buy 10 Supplies (for 50 Gold)");
                        Console.WriteLine("8. Start Adventuring Phase");
                        Console.WriteLine("9. Save Game");
                        Console.WriteLine("0. Close Game");
                        Console.Write("Enter your choice: ");
                        input = Console.ReadLine();
                        Console.Clear();

                        switch (input)
                        {
                            case "1": // Hire Artisan
                                      // Generate List of Artisans available to hire
                                List<Artisan> availableArtisans = new List<Artisan>();
                                for (int i = 0; i < Guild.MaxArtisansAvailable; i++)
                                {
                                    availableArtisans.Add(new Artisan());
                                    Console.WriteLine($"{i + 1}. Hire {availableArtisans[i].Name} (Crafting Skill: {availableArtisans[i].CraftSkill}, Wage: {availableArtisans[i].Wage} gold)");
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Enter your choice: ");
                                input = Console.ReadLine();
                                Console.Clear();
                                int inputInt = Convert.ToInt32(input);

                                if (inputInt > 0 && inputInt <= Guild.MaxArtisansAvailable)
                                {
                                    // Hire selected artisan
                                    Guild.Artisans.Add(availableArtisans[inputInt - 1]);
                                    Console.WriteLine($"{availableArtisans[inputInt - 1].Name} (Crafting Skill: {availableArtisans[inputInt - 1].CraftSkill}) hired.");
                                    Console.WriteLine();
                                }
                                else if (inputInt == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "2": // Train Artisan
                                for (int i = 0; i < Guild.Artisans.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. Train {Guild.Artisans[i].Name} (Crafting Skill: {Guild.Artisans[i].CraftSkill})");
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Enter your choice: ");
                                input = Console.ReadLine();
                                inputInt = Convert.ToInt32(input);
                                Console.Clear();

                                if (inputInt > 0 && inputInt <= Guild.Artisans.Count)
                                {
                                    // Train Selected Artisan
                                    Guild.TrainArtisan(inputInt - 1);
                                }
                                else if (inputInt == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "3": // Fire Artisan
                                for (int i = 0; i < Guild.Artisans.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. Fire {Guild.Artisans[i].Name} (Crafting Skill: {Guild.Artisans[i].CraftSkill}) for the cost {Guild.Artisans[i].CraftSkill / 10} fame.");
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Enter your choice: ");
                                input = Console.ReadLine();
                                inputInt = Convert.ToInt32(input);
                                Console.Clear();

                                if (inputInt > 0 && inputInt <= Guild.Artisans.Count)
                                {
                                    Guild.FireArtisan(inputInt - 1);
                                }
                                else if (inputInt == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "4": // Hire Adventurer
                                      // Generate List of Adventurers available to hire
                                List<Adventurer> availableAdventurers = new List<Adventurer>();
                                for (int i = 0; i < Guild.MaxAdventurersAvailable; i++)
                                {
                                    availableAdventurers.Add(new Adventurer());
                                    Console.WriteLine($"{i + 1}. Hire {availableAdventurers[i].Name} (HP: {availableAdventurers[i].HealthPoints}," +
                                        $" Strength: {availableAdventurers[i].Strength}," +
                                        $" Armor Class: {availableAdventurers[i].ArmorRating}," +
                                        $" Armor Coverage {availableAdventurers[i].ArmorCoverage}," +
                                        $" Wage: {availableAdventurers[i].Wage} gold)");
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Enter your choice: ");
                                input = Console.ReadLine();
                                Console.Clear();
                                inputInt = Convert.ToInt32(input);

                                if (inputInt > 0 && inputInt <= Guild.MaxAdventurersAvailable)
                                {
                                    // Hire selected adventurer
                                    Guild.Adventurers.Add(availableAdventurers[inputInt - 1]);
                                    Console.WriteLine($"{availableAdventurers[inputInt - 1].Name} hired.");
                                }
                                else if (inputInt == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "5": // Train Adventurer
                                break;
                            case "6": // Fire Adventurer
                                for (int i = 0; i < Guild.Adventurers.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. Fire {Guild.Adventurers[i].Name} for the cost {Guild.Adventurers[i].Wage / 2} fame.");
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Enter your choice: ");
                                input = Console.ReadLine();
                                inputInt = Convert.ToInt32(input);
                                Console.Clear();

                                if (inputInt > 0 && inputInt <= Guild.Adventurers.Count)
                                {
                                    Guild.FireAdventurer(inputInt - 1);
                                }
                                else if (inputInt == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "7": // Buy 10 Supplies (for 50 Gold)
                                Guild.BuySupplies();
                                break;
                            case "8": // Start Adventuring Phase
                                gameState = GameState.ItemProduction;
                                break;
                            case "9": // Save Game
                                SaveGame(gameState);
                                break;
                            case "0": // Close Game
                                gameIsOpened = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Try again.");
                                break;
                        }
                    }
                    else if (gameState == GameState.ItemProduction)
                    {
                        // ## Artisans produce their items
                        Console.WriteLine("I----------ITEM PRODUCTION PHASE----------I");
                        foreach (Artisan artisan in Guild.Artisans)
                        {
                            // Produce item
                            // Add produced item to Guild Inventory
                            Guild.Items.Add(artisan.ProduceItem());
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        gameState = GameState.AdventurePreparation;
                    }
                    else if (gameState == GameState.AdventurePreparation)
                    {
                        Console.WriteLine("I----------ADVENTURE PREPARATION PHASE----------I");
                        Console.WriteLine($"I---{Guild.Name} - Fame: {Guild.Fame}, Gold: {Guild.Gold}, Materials: {Guild.Materials}--I");
                        Console.WriteLine("I-----------------------------------------------I");

                        Console.WriteLine("1. Equip Adventurers");
                        Console.WriteLine("2. Unequip Adventurers");
                        Console.WriteLine("3. Sell Items");
                        Console.WriteLine("4. Present Items to the King");
                        Console.WriteLine("5. Venture Forth!");
                        Console.WriteLine("9. Save Game");
                        Console.WriteLine("0. Close Game");
                        Console.Write("Enter your choice: ");
                        input = Console.ReadLine();
                        Console.Clear();

                        switch (input)
                        {
                            case "1": // Equip Adventurers
                                      // List Adventurers
                                for (int i = 0; i < Guild.Adventurers.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {Guild.Adventurers[i].Name} (HP: {Guild.Adventurers[i].HealthPoints}," +
                                        $" Strength: {Guild.Adventurers[i].Strength}," +
                                        $" Armor Class: {Guild.Adventurers[i].ArmorRating}," +
                                        $" Armor Coverage {Guild.Adventurers[i].ArmorCoverage}," +
                                        $" Wage: {Guild.Adventurers[i].Wage} gold)");
                                    Console.Write("Equipped Weapon: ");
                                    if (Guild.Adventurers[i].EquippedWeapon != null)
                                    {
                                        Console.Write($"{Guild.Adventurers[i].EquippedWeapon.ItemName} (Damage: {Guild.Adventurers[i].EquippedWeapon.MinDamage}-{Guild.Adventurers[i].EquippedWeapon.MaxDamage}, " +
                                        $"Quality: {Guild.Adventurers[i].EquippedWeapon.QualityRating}, Elegance: {Guild.Adventurers[i].EquippedWeapon.EleganceRating}).");
                                    }
                                    Console.WriteLine();
                                    Console.Write("Equipped Armor: ");
                                    if (Guild.Adventurers[i].EquippedArmor != null)
                                    {
                                        Console.Write($"{Guild.Adventurers[i].EquippedArmor.ItemName}  (Rating:  {Guild.Adventurers[i].EquippedArmor.ArmorRating} , Coverage: {Guild.Adventurers[i].EquippedArmor.ArmorCoverage}, " +
                                        $"Quality: {Guild.Adventurers[i].EquippedArmor.QualityRating} , Elegance:  {Guild.Adventurers[i].EquippedArmor.EleganceRating}).");
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine();
                                }

                                // List all items in the Guild Inventory
                                Guild.ListItems();

                                Console.WriteLine("0. Back");
                                Console.Write("Choose Adventurer: ");
                                input = Console.ReadLine();
                                //Console.Clear();
                                int inputChoiceAdventurer = Convert.ToInt32(input);

                                if (inputChoiceAdventurer > 0 && inputChoiceAdventurer <= Guild.Adventurers.Count)
                                {
                                    // Equip selected adventurer with selected item
                                    Console.Write("Choose Item to be equipped: ");
                                    input = Console.ReadLine();
                                    Console.Clear();
                                    int inputChoiceItem = Convert.ToInt32(input);

                                    if (inputChoiceItem > 0 && inputChoiceItem <= Guild.Items.Count)
                                    {
                                        Guild.Adventurers[inputChoiceAdventurer - 1].EquipItem(Guild.Items[inputChoiceItem - 1]);
                                    }
                                    else
                                    {
                                        // Default
                                        Console.WriteLine("Invalid choice. Try again.");
                                    }
                                }
                                else if (inputChoiceAdventurer == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "2": // Unequip Adventurers
                                      // List Adventurers
                                for (int i = 0; i < Guild.Adventurers.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {Guild.Adventurers[i].Name} (HP: {Guild.Adventurers[i].HealthPoints}," +
                                        $" Strength: {Guild.Adventurers[i].Strength}," +
                                        $" Armor Class: {Guild.Adventurers[i].ArmorRating}," +
                                        $" Armor Coverage {Guild.Adventurers[i].ArmorCoverage}," +
                                        $" Wage: {Guild.Adventurers[i].Wage} gold)");
                                    Console.Write("Equipped Weapon: ");
                                    if (Guild.Adventurers[i].EquippedWeapon != null)
                                    {
                                        Console.Write($"{Guild.Adventurers[i].EquippedWeapon.ItemName} (Damage: {Guild.Adventurers[i].EquippedWeapon.MinDamage}-{Guild.Adventurers[i].EquippedWeapon.MaxDamage}, " +
                                        $"Quality: {Guild.Adventurers[i].EquippedWeapon.QualityRating}, Elegance: {Guild.Adventurers[i].EquippedWeapon.EleganceRating}).");
                                    }
                                    Console.WriteLine();
                                    Console.Write("Equipped Armor: ");
                                    if (Guild.Adventurers[i].EquippedArmor != null)
                                    {
                                        Console.Write($"{Guild.Adventurers[i].EquippedArmor.ItemName}  (Rating:  {Guild.Adventurers[i].EquippedArmor.ArmorRating} , Coverage: {Guild.Adventurers[i].EquippedArmor.ArmorCoverage}, " +
                                        $"Quality: {Guild.Adventurers[i].EquippedArmor.QualityRating} , Elegance:  {Guild.Adventurers[i].EquippedArmor.EleganceRating}).");
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine();
                                }
                                Console.WriteLine("0. Back");
                                Console.Write("Choose Adventurer: ");
                                input = Console.ReadLine();
                                inputChoiceAdventurer = Convert.ToInt32(input);

                                if (inputChoiceAdventurer > 0 && inputChoiceAdventurer <= Guild.Adventurers.Count)
                                {
                                    // Unequip selected adventurer's weapon or armor
                                    Console.Write("Unequip weapon or armor (w / a): ");
                                    string inputChoiceItem = Console.ReadLine();
                                    Console.Clear();

                                    if (inputChoiceItem == "w")
                                    {
                                        // Unequip weapon
                                        if (Guild.Adventurers[inputChoiceAdventurer - 1].EquippedWeapon != null)
                                        {
                                            Guild.Adventurers[inputChoiceAdventurer - 1].UnequipItem("Weapon");
                                        }
                                        else
                                        {
                                            Console.WriteLine("There is no weapon equipped.");
                                        }
                                    }
                                    else if (inputChoiceItem == "a")
                                    {
                                        // Unequip armor
                                        if (Guild.Adventurers[inputChoiceAdventurer - 1].EquippedArmor != null)
                                        {
                                            Guild.Adventurers[inputChoiceAdventurer - 1].UnequipItem("Armor");
                                        }
                                        else
                                        {
                                            Console.WriteLine("There is no armor equipped.");
                                        }
                                    }
                                    else
                                    {
                                        // Default
                                        Console.WriteLine("Invalid choice. Try again.");
                                    }
                                }
                                else if (inputChoiceAdventurer == 0)
                                {
                                    // Back
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }
                                break;
                            case "3": // Sell Items in the Inventory
                                      // List all items in the Guild Inventory
                                Guild.ListItems();

                                // Sell selected item
                                Console.Write("Choose Item to be sold: ");
                                input = Console.ReadLine();
                                Console.Clear();
                                int inputChoiceSold = Convert.ToInt32(input);

                                if (inputChoiceSold > 0 && inputChoiceSold <= Guild.Items.Count)
                                {
                                    Guild.SellItem(inputChoiceSold - 1);
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }

                                break;
                            case "4": // Present Items to the King
                                      // List all items in the Guild Inventory
                                Guild.ListItems();

                                // Sell selected item
                                Console.Write("Choose Item to be presented to the King: ");
                                input = Console.ReadLine();
                                Console.Clear();
                                int inputChoiceShowcased = Convert.ToInt32(input);

                                if (inputChoiceShowcased > 0 && inputChoiceShowcased <= Guild.Items.Count)
                                {
                                    Guild.ShowcaseItem(inputChoiceShowcased - 1);
                                }
                                else
                                {
                                    // Default
                                    Console.WriteLine("Invalid choice. Try again.");
                                }

                                break;
                            case "5": // Venture Forth
                                Console.WriteLine("Venture Forth!");
                                gameState = GameState.Adventuring;
                                break;
                            case "9": // Save Game
                                SaveGame(gameState);
                                break;
                            case "0": // Close Game
                                gameIsOpened = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Try again.");
                                break;
                        }
                    }
                    else if (gameState == GameState.Adventuring)
                    {
                        // Adventuring Phase
                        Console.WriteLine("I----------ADVENTURING PHASE----------I");
                        Console.WriteLine($"I---{Guild.Name} - Fame: {Guild.Fame}, Gold: {Guild.Gold}, Materials: {Guild.Materials}--I");
                        Console.WriteLine("I-------------------------------------I");

                        Guild.BattlePhase();

                        // Pay Day
                        Console.WriteLine("Pay day!");
                        Guild.PayWages();
                        Console.WriteLine();

                        Console.WriteLine("Start the next turn (Press any key)");

                        // Next turn, Artisans can be trained again
                        foreach (Artisan artisan in Guild.Artisans)
                        {
                            artisan.TrainedThisTurn = false;
                        }

                        Console.ReadKey();
                        gameState = GameState.GuildManagement;
                        Console.Clear();
                    }
                }
            }
        }
    }
}