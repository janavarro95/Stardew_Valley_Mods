using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.StardustCore.Utilities.Objects
{
    /// <summary>
    /// A util class for dealing with objects.
    /// </summary>
    public class ObjectIds
    {

        public enum StardewObjectIds
        {
            //Weeds = 0,

            //Stone = 2,

            //Stone = 4,

            WildHorseradish = 16,

            Daffodil = 18,

            Leek = 20,

            Dandelion = 22,

            Parsnip = 24,

            //Lumber = 30,

            Emerald = 60,

            Aquamarine = 62,

            Ruby = 64,

            Amethyst = 66,

            Topaz = 68,

            Jade = 70,

            Diamond = 72,

            PrismaticShard = 74,

            //Stone = 75,

            //Stone = 76,

            //Stone = 77,

            CaveCarrot = 78,

            //SecretNote = 79,

            Quartz = 80,

            FireQuartz = 82,

            FrozenTear = 84,

            EarthCrystal = 86,

            Coconut = 88,

            CactusFruit = 90,

            Sap = 92,

            Torch = 93,

            SpiritTorch = 94,

            DwarfScrollI = 96,

            DwarfScrollII = 97,

            DwarfScrollIII = 98,

            DwarfScrollIV = 99,

            ChippedAmphora = 100,

            Arrowhead = 101,

            LostBook = 102,

            AncientDoll = 103,

            ElvishJewelry = 104,

            ChewingStick = 105,

            OrnamentalFan = 106,

            DinosaurEgg = 107,

            RareDisc = 108,

            AncientSword = 109,

            RustySpoon = 110,

            RustySpur = 111,

            RustyCog = 112,

            ChickenStatue = 113,

            AncientSeed = 114,

            PrehistoricTool = 115,

            DriedStarfish = 116,

            Anchor = 117,

            GlassShards = 118,

            BoneFlute = 119,

            PrehistoricHandaxe = 120,

            DwarvishHelm = 121,

            DwarfGadget = 122,

            AncientDrum = 123,

            GoldenMask = 124,

            GoldenRelic = 125,

            StrangeDoll1 = 126,

            StrangeDoll2 = 127,

            Pufferfish = 128,

            Anchovy = 129,

            Tuna = 130,

            Sardine = 131,

            Bream = 132,

            LargemouthBass = 136,

            SmallmouthBass = 137,

            RainbowTrout = 138,

            Salmon = 139,

            Walleye = 140,

            Perch = 141,

            Carp = 142,

            Catfish = 143,

            Pike = 144,

            Sunfish = 145,

            RedMullet = 146,

            Herring = 147,

            Eel = 148,

            Octopus = 149,

            RedSnapper = 150,

            Squid = 151,

            Seaweed = 152,

            GreenAlgae = 153,

            SeaCucumber = 154,

            SuperCucumber = 155,

            Ghostfish = 156,

            WhiteAlgae = 157,

            Stonefish = 158,

            Crimsonfish = 159,

            Angler = 160,

            IcePip = 161,

            LavaEel = 162,

            Legend = 163,

            Sandfish = 164,

            ScorpionCarp = 165,

            TreasureChest = 166,

            JojaCola = 167,

            Trash = 168,

            Driftwood = 169,

            BrokenGlasses = 170,

            BrokenCD = 171,

            SoggyNewspaper = 172,

            Egg = 176,

            LargeEgg = 174,

            Hay = 178,

            BrownEgg = 180,

            LargeBrownEgg = 182,

            Milk = 184,

            LargeMilk = 186,

            GreenBean = 188,

            Cauliflower = 190,

            Potato = 192,

            FriedEgg = 194,

            Omelet = 195,

            Salad = 196,

            CheeseCauliflower = 197,

            BakedFish = 198,

            ParsnipSoup = 199,

            VegetableMedley = 200,

            CompleteBreakfast = 201,

            FriedCalamari = 202,

            StrangeBun = 203,

            LuckyLunch = 204,

            FriedMushroom = 205,

            Pizza = 206,

            BeanHotpot = 207,

            GlazedYams = 208,

            CarpSurprise = 209,

            Hashbrowns = 210,

            Pancakes = 211,

            SalmonDinner = 212,

            FishTaco = 213,

            CrispyBass = 214,

            PepperPoppers = 215,

            Bread = 216,

            TomKhaSoup = 218,

            TroutSoup = 219,

            ChocolateCake = 220,

            PinkCake = 221,

            RhubarbPie = 222,

            Cookie = 223,

            Spaghetti = 224,

            FriedEel = 225,

            SpicyEel = 226,

            Sashimi = 227,

            MakiRoll = 228,

            Tortilla = 229,

            RedPlate = 230,

            EggplantParmesan = 231,

            RicePudding = 232,

            IceCream = 233,

            BlueberryTart = 234,

            AutumnsBounty = 235,

            PumpkinSoup = 236,

            SuperMeal = 237,

            CranberrySauce = 238,

            Stuffing = 239,

            FarmersLunch = 240,

            SurvivalBurger = 241,

            DishOTheSea = 242,

            MinersTreat = 243,

            RootsPlatter = 244,

            Sugar = 245,

            WheatFlour = 246,

            Oil = 247,

            Garlic = 248,

            Kale = 250,

            Rhubarb = 252,

            Melon = 254,

            Tomato = 256,

            Morel = 257,

            Blueberry = 258,

            FiddleheadFern = 259,

            HotPepper = 260,

            Wheat = 262,

            Radish = 264,

            RedCabbage = 266,

            Starfruit = 268,

            Corn = 270,

            Eggplant = 272,

            Artichoke = 274,

            Pumpkin = 276,

            BokChoy = 278,

            MagicRockCandy = 279,

            Yam = 280,

            Chanterelle = 281,

            Cranberries = 282,

            Holly = 283,

            Beet = 284,

            CherryBomb = 286,

            Bomb = 287,

            MegaBomb = 288,

            //Stone = 290,

            //Twig = 294,

            //Twig = 295,

            Salmonberry = 296,

            GrassStarter = 297,

            HardwoodFence = 298,

            AmaranthSeeds = 299,

            Amaranth = 300,

            GrapeStarter = 301,

            HopsStarter = 302,

            PaleAle = 303,

            Hops = 304,

            VoidEgg = 305,

            Mayonnaise = 306,

            DuckMayonnaise = 307,

            VoidMayonnaise = 308,

            Acorn = 309,

            MapleSeed = 310,

            PineCone = 311,

            //Weeds = 313,

            //Weeds = 314,

            //Weeds = 315,

            //Weeds = 316,

            //Weeds = 317,

            //Weeds = 318,

            //Weeds = 319,

            //Weeds = 320,

            //Weeds = 321,

            WoodFence = 322,

            StoneFence = 323,

            IronFence = 324,

            Gate = 325,

            DwarvishTranslationGuide = 326,

            WoodFloor = 328,

            StoneFloor = 329,

            Clay = 330,

            WeatheredFloor = 331,

            CrystalFloor = 333,

            CopperBar = 334,

            IronBar = 335,

            GoldBar = 336,

            IridiumBar = 337,

            RefinedQuartz = 338,

            Honey = 340,

            TeaSet = 341,

            Pickles = 342,

            //Stone = 343,

            Jelly = 344,

            Beer = 346,

            RareSeed = 347,

            Wine = 348,

            EnergyTonic = 349,

            Juice = 350,

            MuscleRemedy = 351,

            BasicFertilizer = 368,

            QualityFertilizer = 369,

            BasicRetainingSoil = 370,

            QualityRetainingSoil = 371,

            Clam = 372,

            GoldenPumpkin = 373,

            CopperOre = 378,

            IronOre = 380,

            Coal = 382,

            GoldOre = 384,

            IridiumOre = 386,

            Wood = 388,

            Stone = 390,

            NautilusShell = 392,

            Coral = 393,

            RainbowShell = 394,

            Coffee = 395,

            SpiceBerry = 396,

            SeaUrchin = 397,

            Grape = 398,

            SpringOnion = 399,

            Strawberry = 400,

            StrawFloor = 401,

            SweetPea = 402,

            FieldSnack = 403,

            CommonMushroom = 404,

            WoodPath = 405,

            WildPlum = 406,

            GravelPath = 407,

            Hazelnut = 408,

            CrystalPath = 409,

            Blackberry = 410,

            CobblestonePath = 411,

            WinterRoot = 412,

            BlueSlimeEgg = 413,

            CrystalFruit = 414,

            SteppingStonePath = 415,

            SnowYam = 416,

            SweetGemBerry = 417,

            Crocus = 418,

            Vinegar = 419,

            RedMushroom = 420,

            Sunflower = 421,

            PurpleMushroom = 422,

            Rice = 423,

            Cheese = 424,

            GoatCheese = 426,

            Cloth = 428,

            Truffle = 430,

            TruffleOil = 432,

            CoffeeBean = 433,

            //Stardrop = 434,

            GoatMilk = 436,

            RedSlimeEgg = 437,

            LargeGoatMilk = 438,

            PurpleSlimeEgg = 439,

            Wool = 440,

            ExplosiveAmmo = 441,

            DuckEgg = 442,

            DuckFeather = 444,

            RabbitsFoot = 446,

            StoneBase = 449,

            //Stone = 450,

            //Weeds = 452,

            AncientFruit = 454,

            AlgaeSoup = 456,

            PaleBroth = 457,

            Bouquet = 458,

            Mead = 459,

            //MermaidsPendant = 460,

            DecorativePot = 461,

            DrumBlock = 463,

            FluteBlock = 464,

            SpeedGrow = 465,

            DeluxeSpeedGrow = 466,

            ParsnipSeeds = 472,

            BeanStarter = 473,

            CauliflowerSeeds = 474,

            PotatoSeeds = 475,

            GarlicSeeds = 476,

            KaleSeeds = 477,

            RhubarbSeeds = 478,

            MelonSeeds = 479,

            TomatoSeeds = 480,

            BlueberrySeeds = 481,

            PepperSeeds = 482,

            WheatSeeds = 483,

            RadishSeeds = 484,

            RedCabbageSeeds = 485,

            StarfruitSeeds = 486,

            CornSeeds = 487,

            EggplantSeeds = 488,

            ArtichokeSeeds = 489,

            PumpkinSeeds = 490,

            BokChoySeeds = 491,

            YamSeeds = 492,

            CranberrySeeds = 493,

            BeetSeeds = 494,

            SpringSeeds = 495,

            SummerSeeds = 496,

            FallSeeds = 497,

            WinterSeeds = 498,

            AncientSeeds = 499,

            TulipBulb = 427,

            JazzSeeds = 429,

            PoppySeeds = 453,

            SpangleSeeds = 455,

            SunflowerSeeds = 431,

            FairySeeds = 425,

            SmallGlowRing = 516,

            GlowRing = 517,

            SmallMagnetRing = 518,

            MagnetRing = 519,

            SlimeCharmerRing = 520,

            WarriorRing = 521,

            VampireRing = 522,

            SavageRing = 523,

            RingofYoba = 524,

            SturdyRing = 525,

            BurglarsRing = 526,

            IridiumBand = 527,

            JukeboxRing = 528,

            AmethystRing = 529,

            TopazRing = 530,

            AquamarineRing = 531,

            JadeRing = 532,

            EmeraldRing = 533,

            RubyRing = 534,

            Geode = 535,

            FrozenGeode = 536,

            MagmaGeode = 537,

            Alamite = 538,

            Bixite = 539,

            Baryte = 540,

            Aerinite = 541,

            Calcite = 542,

            Dolomite = 543,

            Esperite = 544,

            Fluorapatite = 545,

            Geminite = 546,

            Helvite = 547,

            Jamborite = 548,

            Jagoite = 549,

            Kyanite = 550,

            Lunarite = 551,

            Malachite = 552,

            Neptunite = 553,

            LemonStone = 554,

            Nekoite = 555,

            Orpiment = 556,

            PetrifiedSlime = 557,

            ThunderEgg = 558,

            Pyrite = 559,

            OceanStone = 560,

            GhostCrystal = 561,

            Tigerseye = 562,

            Jasper = 563,

            Opal = 564,

            FireOpal = 565,

            Celestine = 566,

            Marble = 567,

            Sandstone = 568,

            Granite = 569,

            Basalt = 570,

            Limestone = 571,

            Soapstone = 572,

            Hematite = 573,

            Mudstone = 574,

            Obsidian = 575,

            Slate = 576,

            FairyStone = 577,

            StarShards = 578,

            PrehistoricScapula = 579,

            PrehistoricTibia = 580,

            PrehistoricSkull = 581,

            SkeletalHand = 582,

            PrehistoricRib = 583,

            PrehistoricVertebra = 584,

            SkeletalTail = 585,

            NautilusFossil = 586,

            AmphibianFossil = 587,

            PalmFossil = 588,

            Trilobite = 589,

            ArtifactSpot = 590,

            Tulip = 591,

            SummerSpangle = 593,

            FairyRose = 595,

            BlueJazz = 597,

            Sprinkler = 599,

            Poppy = 376,

            PlumPudding = 604,

            ArtichokeDip = 605,

            StirFry = 606,

            RoastedHazelnuts = 607,

            PumpkinPie = 608,

            RadishSalad = 609,

            FruitSalad = 610,

            BlackberryCobbler = 611,

            CranberryCandy = 612,

            Apple = 613,

            GreenTea = 614,

            Bruschetta = 618,

            QualitySprinkler = 621,

            IridiumSprinkler = 645,

            Coleslaw = 648,

            FiddleheadRisotto = 649,

            PoppyseedMuffin = 651,

            CherrySapling = 628,

            ApricotSapling = 629,

            OrangeSapling = 630,

            PeachSapling = 631,

            PomegranateSapling = 632,

            AppleSapling = 633,

            Apricot = 634,

            Orange = 635,

            Peach = 636,

            Pomegranate = 637,

            Cherry = 638,

            //Stone = 668,

            //Stone = 670,

            //Weeds = 674,

            //Weeds = 675,

            //Weeds = 676,

            //Weeds = 677,

            //Weeds = 678,

            //Weeds = 679,

            GreenSlimeEgg = 680,

            RainTotem = 681,

            MutantCarp = 682,

            BugMeat = 684,

            Bait = 685,

            Spinner = 686,

            DressedSpinner = 687,

            WarpTotemFarm = 688,

            WarpTotemMountains = 689,

            WarpTotemBeach = 690,

            BarbedHook = 691,

            LeadBobber = 692,

            TreasureHunter = 693,

            TrapBobber = 694,

            CorkBobber = 695,

            Sturgeon = 698,

            TigerTrout = 699,

            Bullhead = 700,

            Tilapia = 701,

            Chub = 702,

            Magnet = 703,

            Dorado = 704,

            Albacore = 705,

            Shad = 706,

            Lingcod = 707,

            Halibut = 708,

            Hardwood = 709,

            CrabPot = 710,

            Lobster = 715,

            Crayfish = 716,

            Crab = 717,

            Cockle = 718,

            Mussel = 719,

            Shrimp = 720,

            Snail = 721,

            Periwinkle = 722,

            Oyster = 723,

            MapleSyrup = 724,

            OakResin = 725,

            PineTar = 726,

            Chowder = 727,

            LobsterBisque = 730,

            FishStew = 728,

            Escargot = 729,

            MapleBar = 731,

            CrabCakes = 732,

            Woodskip = 734,

            StrawberrySeeds = 745,

            JackOLantern = 746,

            RottenPlant = 747,

            //RottenPlant = 748,

            OmniGeode = 749,

            //Weeds = 750,

            //Stone = 751,

            //Stone = 760,

            //Stone = 762,

            //Stone = 764,

            //Stone = 765,

            Slime = 766,

            BatWing = 767,

            SolarEssence = 768,

            VoidEssence = 769,

            MixedSeeds = 770,

            Fiber = 771,

            OilofGarlic = 772,

            LifeElixir = 773,

            WildBait = 774,

            Glacierfish = 775,

            //Weeds = 784,

            //Weeds = 785,

            //Weeds = 786,

            BatteryPack = 787,

            //LostAxe = 788,

            //LuckyPurpleShorts = 789,

            BerryBasket = 790,

            //Weeds = 792,

            //Weeds = 793,

            //Weeds = 794,

            VoidSalmon = 795,

            Slimejack = 796,

            Pearl = 797,

            MidnightSquid = 798,

            SpookFish = 799,

            Blobfish = 800,

            //WeddingRing = 801,

            CactusSeeds = 802,

            DinosaurMayonnaise = 807,

            //IridiumMilk = 803,

            TeaLeaves = 815,


            TreeFertilizer = 805,
            MovieTicket = 809,
            Roe = 812,
            Caviar = 445,
            SquidInk = 814,
            Flounder = 267,
            SeafoamPudding = 265,
            MidnightCarp = 269,
            MahoganySeed = 292,
            OstrichEgg = 289,
            //GoldenWalnut = 73,
            BananaSapling = 69,
            Banana = 91,
            GoldenCoconut = 791,
            FossilizedSkull = 820,
            FossilizedSpine = 821,
            FossilizedTail = 822,
            FossilizedLeg = 823,
            FossilizedRibs = 824,
            SnakeSkull = 825,
            SnakeVertebrae = 826,
            MummifiedBat = 827,
            MummifiedFrog = 828,
            Ginger = 829,
            TaroRoot = 830,
            TaroTuber = 831,
            Pineapple = 832,
            PineappleSeeds = 833,
            Mango = 834,
            MangoSapling = 835,
            Stingray = 836,
            Lionfish = 837,
            BlueDiscus = 838,
            ThornsRing = 839,
            RusticPlankFloor = 840,
            StoneWalkwayFloor = 841,
            CinderShard = 848,
            MagmaCap = 851,
            DragonTooth = 852,
            CuriosityLure = 856,
            TigerSlimeEgg = 857,
            QiGem = 858,
            PhoenixRing = 863,
            //WarMemento = 864,
            //GourmetTomatoSalt = 865,
            //StardewValleyRose = 866,
            //AdvancedTVRemote = 867,
            //ArcticShard = 868,
            //WrigglingWorm = 869,
            //PiratesLocket = 870,
            FairyDust = 872,
            PiñaColada = 873,
            BugSteak = 874,
            Ectoplasm = 875,
            PrismaticJelly = 876,
            QualityBobber = 877,
            MonsterMusk = 879,
            BoneFragment = 881,
            FiberSeeds = 885,
            //WarpTotemIsland = 886,
            //QiFruit = 889,
            //QiBean = 890,
            MushroomTreeSeed = 891,
            //WarpTotemQisArena = 892,
            FireworksRed = 893,
            FireworksPurple = 894,
            FireworksGreen = 895,
            GalaxySoul = 896,
            PierresMissingStocklist = 897,
            SonofCrimsonfish = 898,
            MsAngler = 899,
            LegendII = 900,
            RadioactiveCarp = 901,
            GlacierfishJr = 902,
            GingerAle = 903,
            BananaPudding = 904,
            MangoStickyRice = 905,
            Poi = 906,
            TropicalCurry = 907,
            MagicBait = 908,
            RadioactiveOre = 909,
            RadioactiveBar = 910,
            //HorseFlute = 911,
            Enricher = 913,
            PressureNozzle = 915,
            QiSeasoning = 917,
            HyperSpeedGro = 918,
            DeluxeFertilizer = 919,
            DeluxeRetainingSoil = 920,
            SquidInkRavioli = 921,
            SlimeCrate = 925,
            CookoutKit = 926,
            CampingStove = 927,
            GoldenEgg = 928,
            Hedge = 929,
            //UnknownItem = 930

        }

        /// <summary>
        /// Gets the qualified object id for one of the registered objects.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetQualifiedObjectIdFromStardewObjectId(StardewObjectIds id)
        {
            return string.Format("{0}{1}","(O)",((int)id).ToString());
        }
    }
}
