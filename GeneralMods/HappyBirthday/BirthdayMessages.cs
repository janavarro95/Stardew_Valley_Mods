using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Omegasis.HappyBirthday
{
    public class BirthdayMessages
    {
        /// <summary>The actual birthday wishes given by an npc.</summary>
        public Dictionary<string, string> birthdayWishes;

        public Dictionary<string, string> spouseBirthdayWishes;

        public Dictionary<string, string> defaultSpouseBirthdayWishes = new Dictionary<string, string>()
        {
            ["Alex"] = "",
            ["Elliott"] = "",
            ["Harvey"] = "",
            ["Sam"] = "",
            ["Sebastian"] = "",
            ["Shane"] = "",
            ["Abigail"] = "",
            ["Emily"] = "",
            ["Haley"] = "",
            ["Leah"] = "",
            ["Maru"] = "",
            ["Penny"] = "",
        };

        /// <summary>Used to contain birthday wishes should the mod not find any available.</summary>
        public Dictionary<string, string> defaultBirthdayWishes = new Dictionary<string, string>()
        {
            ["Robin"] = "Hey @, happy birthday! I'm glad you choose this town to move here to. ",
            ["Demetrius"] = "Happy birthday @! Make sure you take some time off today to enjoy yourself. $h",
            ["Maru"] = "Happy birthday @. I tried to make you an everlasting candle for you, but sadly that didn't work out. Maybe next year right? $h",
            ["Sebastian"] = "Happy birthday @. Here's to another year of chilling. ",
            ["Linus"] = "Happy birthday @. Thanks for visiting me even on your birthday. It makes me really happy. ",
            ["Pierre"] = "Hey @, happy birthday! Hopefully this next year for you will be a great one! ",
            ["Caroline"] = "Happy birthday @. Thank you for all that you've done for our community. I'm sure your parents must be proud of you.$h",
            ["Abigail"] = "Happy birthday @! Hopefully this year we can go on even more adventures together $h!",
            ["Alex"] = "Yo @, happy birthday! Maybe this will be your best year yet.$h",
            ["George"] = "When you get to my age birthdays come and go. Still happy birthday @.",
            ["Evelyn"] = "Happy birthday @. You have grown up to be such a fine individual and I'm sure you'll continue to grow. ",
            ["Lewis"] = "Happy birthday @! I'm thankful for what you have done for the town and I'm sure your grandfather would be proud of you.",
            ["Clint"] = "Hey happy birthday @. I'm sure this year is going to be great for you.",
            ["Penny"] = "Happy birthday @. May you enjoy all of life's blessings this year. ",
            ["Pam"] = "Happy birthday kid. We should have a drink to celebrate another year of life for you! $h",
            ["Emily"] = "I'm sensing a strong positive life energy about you, so it must be your birthday. Happy birthday @!$h",
            ["Haley"] = "Happy birthday @. Hopefully this year you'll get some good presents!$h",
            ["Jas"] = "Happy birthday @. I hope you have a good birthday.",
            ["Vincent"] = "Hey @ have you come to pl...oh it's your birthday? Happy birthday! ",
            ["Jodi"] = "Hello there @. Rumor has it that today is your birthday. In that case, happy birthday!$h",
            ["Kent"] = "Jodi told me that it was your birthday today @. Happy birthday and make sure to cherish every single day.",
            ["Sam"] = "Yo @ happy birthday! We'll have to have a birthday jam session for you some time!$h ",
            ["Leah"] = "Hey @ happy birthday! We should go to the saloon tonight and celebrate!$h ",
            ["Shane"] = "Happy birthday @. Keep working hard and I'm sure this next year for you will be a great one.",
            ["Marnie"] = "Hello there @. Everyone is talking about your birthday today and I wanted to make sure that I wished you a happy birthday as well, so happy birthday! $h ",
            ["Elliott"] = "What a wonderful day isn't it @? Especially since today is your birthday. I tried to make you a poem but I feel like the best way of putting it is simply, happy birthday. $h ",
            ["Gus"] = "Hey @ happy birthday! Hopefully you enjoy the rest of the day and make sure you aren't a stranger at the saloon!",
            ["Dwarf"] = "Happy birthday @. I hope that what I got you is acceptable for humans as well. ",
            ["Wizard"] = "The spirits told me that today is your birthday. In that case happy birthday @. May your year shine bright! ",
            ["Harvey"] = "Hey @, happy birthday! Make sure to come in for a checkup some time to make sure you live many more years! ",
            ["Sandy"] = "Hello there @. I heard that today was your birthday and I didn't want you feeling left out, so happy birthday!",
            ["Willy"] = "Aye @ happy birthday. Looking at you reminds me of ye days when I was just a guppy swimming out to sea. Continue to enjoy them youngin.$h",
            ["Krobus"] = "I have heard that it is tradition to give a gift to others on their birthday. In that case, happy birthday @."
        };

        public BirthdayMessages()
        {
            createBirthdayGreetings();
            loadTranslationStrings();
        }


        public Dictionary<StardewValley.LocalizedContentManager.LanguageCode, Dictionary<string, string>> translatedStrings = new Dictionary<StardewValley.LocalizedContentManager.LanguageCode, Dictionary<string, string>>()
        {
            [StardewValley.LocalizedContentManager.LanguageCode.de] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "",
                ["Mail:birthdayDad"] = "",
                ["Happy Birthday: Star Message"] = "",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            },
            [StardewValley.LocalizedContentManager.LanguageCode.en] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "Dear @,^  Happy birthday sweetheart. It's been amazing watching you grow into the kind, hard working person that I've always dreamed that you would become. I hope you continue to make many more fond memories with the ones you love. ^  Love, Mom ^ P.S. Here's a little something that I made for you. %item object 221 1 %%",
                ["Mail:birthdayDad"] = "Dear @,^  Happy birthday kiddo. It's been a little quiet around here on your birthday since you aren't around, but your mother and I know that you are making both your grandpa and us proud.  We both know that living on your own can be tough but we believe in you one hundred percent, just keep following your dreams.^  Love, Dad ^ P.S. Here's some spending money to help you out on the farm. Good luck! %item money 5000 5001 %%",
                ["Happy Birthday: Star Message"] = "It's your birthday today! Happy birthday!",
                ["Happy Birthday: Farmhand Birthday Message"] = "It's @'s birthday! Happy birthday to them!"
            },
            [StardewValley.LocalizedContentManager.LanguageCode.es] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "",
                ["Mail:birthdayDad"] = "",
                ["Happy Birthday: Star Message"] = "",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            },
            [StardewValley.LocalizedContentManager.LanguageCode.ja] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "",
                ["Mail:birthdayDad"] = "",
                ["Happy Birthday: Star Message"] = "",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            },
            [StardewValley.LocalizedContentManager.LanguageCode.pt] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "Caro jogador @,^  Feliz aniversário, docinho. Estou muito orgulhosa de ver como você cresceu, e se tornou aquela pessoa gentil e trabalhadora que eu sempre sonhei que você se tornaria. Espero que você continue a fazer muitas lembranças boas com quem você ama. ^ Com amor, Mãe. ^ P.S. Aqui está uma pequena coisa que fiz para você. %item object 221 1 %%",
                ["Mail:birthdayDad"] = "Caro jogador @,^  Feliz aniversário, garoto(a). Tem sido um pouco quieto por aqui no seu aniversário, já que você não está por perto, mas sua mãe e eu sabemos que você está deixando tanto seu avô quanto nós orgulhosos. Nós dois sabemos que viver sozinho pode ser difícil, mas acreditamos em você cem por cento, apenas continue seguindo seus sonhos. ^ Amor, pai ^ P.S. Aqui está algum dinheiro para ajudá-lo na fazenda. Boa sorte! %item money 5000 5001 %%",
                ["Happy Birthday: Star Message"] = "Hoje é seu aniversário! Parabéns!",
                ["Happy Birthday: Farmhand Birthday Message"] = "Hoje é o aniversário de @. Deem os parabéns para ele(a)."
            },
            [StardewValley.LocalizedContentManager.LanguageCode.ru] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "",
                ["Mail:birthdayDad"] = "",
                ["Happy Birthday: Star Message"] = "",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            },
            [StardewValley.LocalizedContentManager.LanguageCode.th] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "",
                ["Mail:birthdayDad"] = "",
                ["Happy Birthday: Star Message"] = "",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            },
            [StardewValley.LocalizedContentManager.LanguageCode.zh] = new Dictionary<string, string>()
            {
                ["Mail:birthdayMom"] = "亲爱的@，^  生日快乐宝贝。看着你成长成为一个善良努力的人，就如我一直梦想着你成为的样子，我感到十分欣喜。我希望你能继续跟你爱的人制造更多美好的回忆。 ^  爱你的，妈妈 ^ 附言：这是我给你做的一点小礼物。 %item object 221 1 %%",
                ["Mail:birthdayDad"] = "亲爱的@，^  生日快乐孩子。你生日的这天没有你，我们这儿还挺寂寞的，但我和你妈妈都知道你让我们和你爷爷感到骄傲。我们知道你一个人生活可能会很艰难，但我们百分百相信你能做到，所以继续追求你的梦想吧。^  爱你的，爸爸 ^ 附言：这是能在农场上帮到你的一些零用钱。祝你好运！ %item money 5000 5001 %%",
                ["Happy Birthday: Star Message"] = "今天是你的生日！生日快乐！",
                ["Happy Birthday: Farmhand Birthday Message"] = ""
            }
        };




        /// <summary>Used to load all of the default birthday greetings.</summary>
        public void createBirthdayGreetings()
        {
            var serializer = JsonSerializer.Create();
            serializer.Formatting = Formatting.Indented;

            //English logic.
            string defaultPath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue", HappyBirthday.Config.translationInfo.currentTranslation);
            if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);

            string birthdayFileDict = HappyBirthday.Config.translationInfo.getjsonForTranslation("BirthdayWishes", HappyBirthday.Config.translationInfo.currentTranslation);
            string path = Path.Combine("Content", "Dialogue", HappyBirthday.Config.translationInfo.currentTranslation, birthdayFileDict);

            //Handle normal birthday wishes.
            if (!File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, path)))
            {
                HappyBirthday.ModMonitor.Log("Creating Villager Birthday Messages", StardewModdingAPI.LogLevel.Alert);
                HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(path, this.defaultBirthdayWishes);
                this.birthdayWishes = this.defaultBirthdayWishes;
            }
            else
                this.birthdayWishes = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(path);

            //handle spouse birthday wishes.
            string spouseBirthdayFileDict = HappyBirthday.Config.translationInfo.getjsonForTranslation("SpouseBirthdayWishes", HappyBirthday.Config.translationInfo.currentTranslation);
            string spousePath = Path.Combine("Content", "Dialogue", HappyBirthday.Config.translationInfo.currentTranslation, spouseBirthdayFileDict);
            if (!File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, spousePath)))
            {
                HappyBirthday.ModMonitor.Log("Creating Spouse Messages", StardewModdingAPI.LogLevel.Alert);
                HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(spousePath, this.defaultSpouseBirthdayWishes);
                this.spouseBirthdayWishes = this.defaultSpouseBirthdayWishes;
            }
            else
                this.spouseBirthdayWishes = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(spousePath);

            //Non-english logic for creating templates.
            foreach (var translation in HappyBirthday.Config.translationInfo.translationCodes)
            {
                if (translation.Key == "English")
                    continue;

                string basePath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue", translation.Key);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);
                string tempBirthdayFile = Path.Combine("Content", "Dialogue", translation.Key, HappyBirthday.Config.translationInfo.getjsonForTranslation("BirthdayWishes", translation.Key));
                string tempSpouseBirthdayFile = Path.Combine("Content", "Dialogue", translation.Key, HappyBirthday.Config.translationInfo.getjsonForTranslation("SpouseBirthdayWishes", translation.Key));


                Dictionary<string, string> tempBirthdayDict = new Dictionary<string, string>();
                if (!File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, tempBirthdayFile)))
                {
                    foreach (var pair in this.defaultBirthdayWishes)
                        tempBirthdayDict.Add(pair.Key, "");
                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(tempBirthdayFile, tempBirthdayDict);
                }
                else
                    tempBirthdayDict = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(tempBirthdayFile);


                Dictionary<string, string> tempSpouseBirthdayDict = new Dictionary<string, string>();
                if (!File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, tempSpouseBirthdayFile)))
                {
                    foreach (var pair in this.defaultSpouseBirthdayWishes)
                        tempSpouseBirthdayDict.Add(pair.Key, "");
                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(tempSpouseBirthdayFile, tempSpouseBirthdayDict);
                }
                else
                    tempSpouseBirthdayDict = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(tempSpouseBirthdayFile);

                //Set translated birthday info.
                if (HappyBirthday.Config.translationInfo.currentTranslation == translation.Key)
                {
                    this.birthdayWishes = tempBirthdayDict;
                    this.spouseBirthdayWishes = tempSpouseBirthdayDict;
                    HappyBirthday.ModMonitor.Log("Language set to: " + translation);
                }
            }
        }

        public static string GetTranslatedString(string key)
        {
            StardewValley.LocalizedContentManager.LanguageCode code = HappyBirthday.Config.translationInfo.translationCodes[HappyBirthday.Config.translationInfo.currentTranslation];
            string value= HappyBirthday.Instance.messages.translatedStrings[code][key];
            if (string.IsNullOrEmpty(value))
            {
                return GetEnglishMessageString(key);
            }
            else return value;
        }

        public static string GetEnglishMessageString(string key)
        {
            StardewValley.LocalizedContentManager.LanguageCode code = StardewValley.LocalizedContentManager.LanguageCode.en;
            return HappyBirthday.Instance.messages.translatedStrings[code][key];
        }

        public void loadTranslationStrings()
        {

            //Non-english logic for creating templates.
            foreach (var translation in HappyBirthday.Config.translationInfo.translationCodes)
            {

                StardewValley.LocalizedContentManager.LanguageCode code = translation.Value;

                string basePath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue", translation.Key);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);
                string stringsFile = Path.Combine("Content", "Dialogue", translation.Key, HappyBirthday.Config.translationInfo.getjsonForTranslation("TranslatedStrings", translation.Key));


                if (!File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, stringsFile)))
                {
                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(stringsFile, this.translatedStrings[code]);
                }
                else
                    this.translatedStrings[code] = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(stringsFile);

            }
        }

    }
}
