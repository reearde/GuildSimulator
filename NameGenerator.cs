using System;

namespace GuildSimulator
{
    internal class NameGenerator
    {
        private static readonly Random random = new Random();

        private static readonly string[] namePrefixes = { "Cath", "El", "Il", "Mat", "Is", "Will", "Rob", "Edw", "Arth", "Rich",
            "Al", "Asg", "Bj", "Er", "Fenr", "Har", "Ingm", "Jurg", "Kj", "Moj", "Sor", "Torb", "Ulfr", "An", "Bet", "Dor",
            "Ell", "Han", "Hell", "Ing", "Jyt", "Kirst", "Met", "Morg", "Sill", "Ull" };
        private static readonly string[] nameSuffixes = { "anor", "ella", "ilda", "erine", "eth", "iam", "art", "ert", "ur", 
            "ald", "an", "ar", "arik", "erik", "arke", "arne", "eld", "en", "ens", "er", "ik", "is", "orn", "a", "e", "en",
            "ia", "ina", "ne", "te"};
        private static readonly string[] lastNamePrefixes = { "Black", "White", "Grey", "Iron", "Steel", "Haw", "Stone", "Raven", "Crow",
            "Frost", "Fire", "Silver", "Gold", "Far", "Half", "High", "Low", "Deep", "Tallow", "Candle", "East", "West", "Swift",
            "North", "South", "Gentle", "Strong", "Lean", "Long", "Stout", "Rascal", "Oaken", "Dark", "Forest", "Snow", "Last",
            "Cold", "Ash", "Copper", "Green", "Hawk"};
        private static readonly string[] lastNameSuffixes = { "wood", "side", "thorne", "wall", "leaf", "lander", "hand", "foot", "arm",
            "tooth", "hair", "mouth", "fang", "eye", "er", "seal", "song", "beard", "nail", "blood", "sayer", "shield", "sword",
            "claw", "mane", "fist", "walker", "rock", "moon", "ford", "croft", "field", "hart", "house", "wing", "ton", "ley"};

        public static string GenerateName()
        {
            string namePrefix = namePrefixes[random.Next(namePrefixes.Length)];
            string nameSuffix = nameSuffixes[random.Next(nameSuffixes.Length)];
            string lastNamePrefix = lastNamePrefixes[random.Next(lastNamePrefixes.Length)];
            string lastNameSuffix = lastNameSuffixes[random.Next(lastNameSuffixes.Length)];

            return $"{namePrefix}{nameSuffix} {lastNamePrefix}{lastNameSuffix}";
        }

        private static readonly string[] armorNames = { "Breast Plate", "Cape", "Carapace", "Chain Mail", "Cloak", "Coat", "Full-plate Armor",
            "Half-plate Armor", "Hard Leather Armor", "Leather Armor", "Mail Armor", "Quilted Armor", "Rags", "Ring Mail", "Robe",
            "Scale Mail", "Splint Mail", "Studded Leather Armor" };

        public static string GenerateArmorName()
        {
            string armorName = armorNames[random.Next(armorNames.Length)];

            return $"{armorName}";
        }

        private static readonly string[] weaponNames = { "Axe", "Battle Axe", "Blade", "Broad Axe", "Broad Sword", "Claymore", "Cleaver", "Club",
            "Cudgel", "Dagger", "Dirk", "Falchion", "Flail", "Gladius", "Glaive", "Great Axe", "Great Sword", "Halberd", "Hammer", "Hatchet",
            "Katana", "Knife", "Kris", "Kunai", "Long Sword", "Mace", "Machete", "Maul", "Morningstar", "Razor", "Sabre", "Scimitar", "Shortsword",
            "Sword", "Spiked Club", "Tanto", "Warhammer", "Zweihander", "Bow", "Short Bow", "Long Bow", "Composite Bow", "Hunter's Bow", "Crossbow",
            "Arbalest", "Staff", "Short Staff", "Long Staff", "Quarterstaff", "Cutlass", "Spear", "Polearm", "Tiger Claws", "Knuckles", "Cestus",
            "Katar", "Push Dagger", "Karabela", "Arming Sword", "Estoc", "Flamberge", "Rapier", "Tachi", "Wakizashi", "Kodachi", "Kama", "Xiphos",
            "Yatagan", "Karambit", "Sickle", "Pickaxe", "Mattock", "Knightly Sword", "Adze", "Bardiche", "Tabarzin", "Tomahawk", "Bo", "Boomerang",
            "Bludgeon", "Sai", "Sledgehammer", "Scepter", "Javelin", "Lance", "Pike", "Pitchfork", "Trident", "Scythe", "Chakram", "Shuriken",
            "Throwing Knife", "Throwing Dart", "Harpoon", "Pilum", "Arquebus", "Blunderbuss", "Handcannon", "Handmortar", "Musket", "Pistol Sword",
            "Flintlock Pistol", "Matchlock Pistol", "Sling", "Whip", "Nunchaku" };

        public static string GenerateWeaponName()
        {
            string weaponName = weaponNames[random.Next(weaponNames.Length)];

            return $"{weaponName}";
        }

        private static readonly string[] enemiesMinion = { "Goblin", "Orc", "Fiend", "Rat", "Bat", "Bandit", "Bandit Archer", "Outlaw", "Kobold",
            "Spiderling", "Imp", "Skeleton", "Skeleton Warrior", "Skeleton Archer", "Worm", "Wolf", "Acolyte", "Cultist", "Zealot", "Bat", "Boar",
            "Bear", "Cave Bear", "Hound", "Viper", "Wraith", "Hyena", "Thug", "Marauder", "Slime", "Gnoll", "Hired Thug" };

        private static readonly string[] enemiesRegular = { "Ogre", "Minotaur", "Bandit Captain", "Drake", "Golem", "Spider", "Naga", "Demon", "Troll",
            "Warlock", "Draugr", "Vampire", "Werewolf", "Wereboar", "Warg", "Witch", "Giant", "Bugbear", "Siren", "Harpy", "Hobgoblin", "Hellhound",
            "Rat King", "Cyclops", "Gargoyle", "Hag", "Balrog", "Abomination"};

        private static readonly string[] enemiesBoss = { "Dragon", "Fire Dragon", "Ice Dragon", "Ancient Dragon", "Leviathan", "Lich", "Gorgon", "Baphomet",
            "Diablo", "Mephisto", "Bhaal", "Colossus", "Kraken", "Sentinel", "Necromancer", "Beholder", "Vampire Lord", "Bandit Leader", "Anansi", "Griffin",
            "Cerberus", "Baba Yaga", "Titan", "Hydra", "Basilisk", "Cthulhu"};

        public static string GenerateEnemyName(string enemyType)
        {
            string enemyName = "";
            if (enemyType == "Minion")
            {
                enemyName = enemiesMinion[random.Next(enemiesMinion.Length)];
            }
            else if (enemyType == "Regular")
            {
                enemyName = enemiesRegular[random.Next(enemiesRegular.Length)];
            }
            else if (enemyType == "Boss")
            {
                enemyName = enemiesBoss[random.Next(enemiesBoss.Length)];
            }
            return $"{enemyName}";
        }
    }
}