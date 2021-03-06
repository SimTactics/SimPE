/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;

namespace SimPe.Data
{
	public enum NeighborhoodSlots
	{
		LotsIntern = 0,
		Lots = 1,
		FamiliesIntern = 2,
		Families = 3,
		SimsIntern = 4,
		Sims = 5
	}
	/// <summary>
	/// Determins the concrete Type of an Overlay Item (texture or mesh overlay)
	/// </summary>
	public enum TextureOverlayTypes : uint
	{
		Beard = 0x00,
		EyeBrow = 0x01,
		Lipstick = 0x02,
        Eye = 0x03,
		Mask = 0x04,
		Glasses = 0x05,
		Blush = 0x06,
        EyeShadow = 0x07
	}
	
	/// <summary>
	/// Ages used for Property Sets (Character Data, Skins)
	/// </summary>
	public enum Ages:uint
	{
		Baby = 0x20,
		Toddler = 0x01,
		Child = 0x02,
		Teen = 0x04,
		Adult = 0x08,
		Elder = 0x10,
		YoungAdult = 0x40
	}

	/// <summary>
	/// Categories used for Property Sets (Skins) (Updated by Theo
	/// </summary>
    [Flags]
    public enum SkinCategories:uint
    {
        Casual1 = 0x01,
        Casual2 = 0x02,
        Casual3 = 0x04,
        Everyday = Casual1 | Casual2 | Casual3,
        Swimmwear = 0x08,
        PJ = 0x10,
        Formal = 0x20,
        Undies = 0x40,
        Skin = 0x80,
        Pregnant = 0x100,
        Activewear = 0x200,
        TryOn = 0x400,
        NakedOverlay = 0x800,
        Outerwear = 0x1000
    }

	/// <summary>
	/// 
	/// </summary>
	public enum Majors:uint 
	{
		Unset = 0,
		Unknown = 0xffffffff,
		Art = 0x2e9cf007,
		Biology = 0x4e9cf02b,
		Drama = 0x4e9cf04d,
		Economics = 0xEe9cf044,
		History = 0x2e9cf074,
		Literature = 0xce9cf085,
		Mathematics = 0xee9cf08d,
		Philosophy = 0x2e9cf057,
		Physics = 0xae9cf063,
		PoliticalScience = 0x4e9cf06d,
		Psychology = 0xCE9CF07C,
		Undeclared = 0x8e97bf1d
	}

	/// <summary>
	/// Room Sort Flag
	/// </summary>
	public enum ObjRoomSortBits:byte
	{
		Kitchen = 0x00,
		Bedroom = 0x01,
		Bathroom = 0x02,
		LivingRoom = 0x03,
		Outside = 0x04,
		DiningRoom = 0x05,
		Misc = 0x06,
		Study = 0x07,
		Kids = 0x08
	}

	/// <summary>
	/// Function Sort Flag 
	/// </summary>
	public enum ObjFunctionSortBits:byte
	{
		Seating = 0x00,
		Surfaces = 0x01,
		Appliances = 0x02,
		Electronics = 0x03,
		Plumbing = 0x04,
		Decorative = 0x05,
		General = 0x06,
		Lighting = 0x07,
		Hobbies = 0x08,
		AspirationRewards = 0x0a,
		CareerRewards = 0x0b
	}

	/// <summary>
	/// Function for xml Based Objects
	/// </summary>	
	public enum XObjFunctionSubSort:uint
	{
		Roof = 0x0100,

		Floor_Brick = 0x0201,
		Floor_Carpet = 0x0202,
		Floor_Lino = 0x0204,
		Floor_Poured = 0x0208,
		Floor_Stone = 0x0210,
		Floor_Tile = 0x0220,
		Floor_Wood = 0x0240,
		Floor_Other = 0x0200,

		Fence_Rail = 0x0400,
		Fence_Halfwall = 0x0401,

		Wall_Brick = 0x0501,
		Wall_Masonry = 0x0502,
		Wall_Paint = 0x0504,
		Wall_Paneling = 0x0508,
		Wall_Poured = 0x0510,
		Wall_Siding = 0x0520,
		Wall_Tile = 0x0540,
		Wall_Wallpaper = 0x0580,
		Wall_Other = 0x0500,

		Terrain = 0x0600,

		Hood_Landmark = 0x0701,
		Hood_Flora = 0x0702,
		Hood_Effects = 0x0703,
		Hood_Misc = 0x0704,
		Hood_Stone = 0x0705,
		Hood_Other = 0x0700
	}

	/// <summary>
	/// Function Sort Flag 
	/// </summary>
	/// <remarks>the higher byte contains the <see cref="ObjFunctionSortBits"/>, the lower one the actual SubSort</remarks>
	public enum ObjFunctionSubSort:uint
	{
		Seating_DiningroomChair = 0x101,
		Seating_LivingroomChair = 0x102,
		Seating_Sofas = 0x104,
		Seating_Beds = 0x108,
		Seating_Recreation = 0x110,
		Seating_UnknownA = 0x120,
		Seating_UnknownB = 0x140,
		Seating_Misc = 0x180,

		Surfaces_Counter = 0x201,
		Surfaces_Table = 0x202,
		Surfaces_EndTable = 0x204,
		Surfaces_Desks = 0x208,
		Surfaces_Coffeetable = 0x210,
		Surfaces_Business = 0x220,
		Surfaces_UnknownB = 0x240,
		Surfaces_Misc = 0x280,

		Decorative_Wall = 0x2001,
		Decorative_Sculpture = 0x2002,
		Decorative_Rugs = 0x2004,
		Decorative_Plants = 0x2008,
		Decorative_Mirror = 0x2010,
		Decorative_Curtain = 0x2020,
		Decorative_UnknownB = 0x2040,
		Decorative_Misc = 0x2080,

		Plumbing_Toilet = 0x1001,
		Plumbing_Shower = 0x1002,
		Plumbing_Sink = 0x1004,
		Plumbing_HotTub = 0x1008,
		Plumbing_UnknownA = 0x1010,
		Plumbing_UnknownB = 0x1020,
		Plumbing_UnknownC = 0x1040,
		Plumbing_Misc = 0x1080,

		Appliances_Cooking = 0x401,
		Appliances_Refrigerator = 0x402,
		Appliances_Small = 0x404,
		Appliances_Large = 0x408,
		Appliances_UnknownA = 0x410,
		Appliances_UnknownB = 0x420,
		Appliances_UnknownC = 0x440,
		Appliances_Misc = 0x480,

		Electronics_Entertainment = 0x801,
		Electronics_TV_and_Computer = 0x802,
		Electronics_Audio = 0x804,
		Electronics_Small = 0x808,
		Electronics_UnknownA = 0x810,
		Electronics_UnknownB = 0x820,
		Electronics_UnknownC = 0x840,
		Electronics_Misc = 0x880,
		
		Lighting_TableLamp = 0x8001,
		Lighting_FloorLamp = 0x8002,
		Lighting_WallLamp = 0x8004,
		Lighting_CeilingLamp = 0x8008,
		Lighting_Outdoor = 0x8010,
		Lighting_UnknownA = 0x8020,
		Lighting_UnknownB = 0x8040,
		Lighting_Misc = 0x8080,
		
		Hobbies_Creative = 0x10001,
		Hobbies_Knowledge = 0x10002,
		Hobbies_Excerising = 0x10004,
		Hobbies_Recreation = 0x10008,
		Hobbies_UnknownA = 0x10010,
		Hobbies_UnknownB = 0x10020,
		Hobbies_UnknownC = 0x10040,
		Hobbies_Misc = 0x10080,

		General_UnknownA = 0x4001,
		General_Dresser = 0x4002,
		General_UnknownB = 0x4004,
		General_Party = 0x4008,
		General_Child = 0x4010,
		General_Car = 0x4020,
		General_Pets = 0x4040,
		General_Misc = 0x4080,
				
		AspirationRewards_UnknownA = 0x40001,
		AspirationRewards_UnknownB = 0x40002,
		AspirationRewards_UnknownC = 0x40004,
		AspirationRewards_UnknownD = 0x40008,
		AspirationRewards_UnknownE = 0x40010,
		AspirationRewards_UnknownF = 0x40020,
		AspirationRewards_UnknownG = 0x40040,
		AspirationRewards_UnknownH = 0x40080,

		CareerRewards_UnknownA = 0x80001,
		CareerRewards_UnknownB = 0x80002,
		CareerRewards_UnknownC = 0x80004,
		CareerRewards_UnknownD = 0x80008,
		CareerRewards_UnknownE = 0x80010,
		CareerRewards_UnknownF = 0x80020,
		CareerRewards_UnknownG = 0x80040,
		CareerRewards_UnknownH = 0x80080,
	}

	/// <summary>
	/// Enumerates known Object Types
	/// </summary>
	public enum ObjectTypes:ushort 
	{
		Unknown = 0x0000,
		Person = 0x0002,
		Normal = 0x0004,
		ArchitecturalSupport = 0x0005,
		SimType = 0x0007,
		Door = 0x0008,
		Window = 0x0009,
		Stairs = 0x000A,
		ModularStairs = 0x000B,
		ModularStairsPortal = 0x000C,
		Vehicle = 0x000D,
		Outfit = 0x000E,
		Memory = 0x000F,
		Template = 0x0010,
		Tiles = 0x0013
	}

	/// <summary>
	/// Hold Constants, Enumerations and other Metadata
	/// </summary>
    public class MetaData
    {
        /// <summary>
        /// Color of a Sim that is either Unlinked or does not have Character Data
        /// </summary>
        public static Color SpecialSimColor = Color.FromArgb(0xD0, Color.Black);

        /// <summary>
        /// Color of a Sim that is unlinked
        /// </summary>
        public static Color UnlinkedSim = Color.FromArgb(0xEF, Color.SteelBlue);

        /// <summary>
        /// Color of a NPC Sim
        /// </summary>
        public static Color NPCSim = Color.FromArgb(0xEF, Color.YellowGreen);

        /// <summary>
        /// Color of a Sim that has no Character Data
        /// </summary>
        public static Color InactiveSim = Color.FromArgb(0xEF, Color.LightCoral);

        #region Constants

        /// <summary>
        /// Group for Costum Content
        /// </summary>
        public const UInt32 CUSTOM_GROUP = 0x1C050000;

        /// <summary>
        /// Group for Global Content
        /// </summary>
        public const UInt32 GLOBAL_GROUP = 0x1C0532FA;

        /// <summary>
        /// Group for Local Content
        /// </summary>
        public const UInt32 LOCAL_GROUP = 0xffffffff;

        /// <summary>
        /// A Directory file will have this Type in the fileindex.
        /// </summary>
        public const UInt32 DIRECTORY_FILE = 0xE86B1EEF; //0xEF1E6BE8;

        /// <summary>
        /// Stores the relationship Value for a Sim
        /// </summary>
        public const UInt32 RELATION_FILE = 0xCC364C2A;

        /// <summary>
        /// File Containing Strings
        /// </summary>
        public const UInt32 STRING_FILE = 0x53545223;

        /// <summary>
        /// File Containing Pie Strings
        /// </summary>
        public const UInt32 PIE_STRING_FILE = 0x54544173;

        /// <summary>
        /// File Containing Sim Descriptions
        /// </summary>
        public const UInt32 SIM_DESCRIPTION_FILE = 0xAACE2EFB;

        /// <summary>
        /// Files Containing Sim Images
        /// </summary>
        public const UInt32 SIM_IMAGE_FILE = 0x856DDBAC;

        /// <summary>
        /// The File containing all Family Ties
        /// </summary>
        public const UInt32 FAMILY_TIES_FILE = 0x8C870743;

        /// <summary>
        /// File containing BHAV Informations
        /// </summary>
        public const UInt32 BHAV_FILE = 0x42484156;

        /// <summary>
        /// File containng Global Data
        /// </summary>
        public const UInt32 GLOB_FILE = 0x474C4F42;

        /// <summary>
        /// File Containing Object Data
        /// </summary>
        public const UInt32 OBJD_FILE = 0x4F424A44;

        /// <summary>
        /// File Containing Catalog Strings
        /// </summary>
        public const UInt32 CTSS_FILE = 0x43545353;

        /// <summary>
        /// File Containing Name Maps
        /// </summary>
        public const UInt32 NAME_MAP = 0x4E6D6150;

        /// <summary>
        /// Neighborhood/Memory File Typesss
        /// </summary>
        public const UInt32 MEMORIES = 0x4E474248;


        /// <summary>
        /// Sim DNA
        /// </summary>
        public const uint SDNA = 0xEBFEE33F;

        /// <summary>
        /// Signature identifying a compressed PackedFile
        /// </summary>
        public const ushort COMPRESS_SIGNATURE = 0xFB10;

        public const uint GZPS = 0xEBCF3E27;
        public const uint XWNT = 0xED7D7B4D;
        public const uint REF_FILE = 0xAC506764;
        public const uint IDNO = 0xAC8A7A2E;
        public const uint HOUS = 0x484F5553;
        public const uint SLOT = 0x534C4F54;

        public const uint GMND = 0x7BA3838C;
        public const uint TXMT = 0x49596978;
        public const uint TXTR = 0x1C4A276C;
        public const uint LIFO = 0xED534136;
        public const uint ANIM = 0xFB00791E;
        public const uint SHPE = 0xFC6EB1F7;
        public const uint CRES = 0xE519C933;
        public const uint GMDC = 0xAC4F8687;
        public const uint LDIR = 0xC9C81B9B;
        public const uint LAMB = 0xC9C81BA3;
        public const uint LPNT = 0xC9C81BA9;
        public const uint LSPT = 0xC9C81BAD;

        public const uint MMAT = 0x4C697E5A;
        public const uint XOBJ = 0xCCA8E925;
        public const uint XROF = 0xACA8EA06;
        public const uint XFLR = 0x4DCADB7E;
        public const uint XFNC = 0x2CB230B8;
        public const uint XNGB = 0x6D619378;

        public const uint GLUA = 0x9012468A;
        public const uint OLUA = 0x9012468B;

        #endregion

        #region CEP Strings

        public static string GMND_PACKAGE = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads\\_EnableColorOptionsGMND.package");
        public static string MMAT_PACKAGE = System.IO.Path.Combine(PathProvider.Global.GetExpansion(Expansions.BaseGame).InstallFolder, "TSData\\Res\\Sims3D\\_EnableColorOptionsMMAT.package");
        public static string ZCEP_FOLDER  = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "zCEP-EXTRA");
        public static string CTLG_FOLDER  = System.IO.Path.Combine(PathProvider.Global.GetExpansion(Expansions.BaseGame).InstallFolder, "TSData\\Res\\Catalog\\zCEP-EXTRA");

        #endregion

        #region Enums

        /// <summary>
        /// Type of school a Sim attends
        /// </summary>
        public enum SchoolTypes : uint
        {
            Unknown = 0x00000000,
            PublicSchool = 0xD06788B5,
            PrivateSchool = 0xCC8F4C11
        }

        /// <summary>
        /// Available Grades
        /// </summary>
        public enum Grades : ushort
        {
            Unknown = 0x00,
            F = 0x01,
            DMinus = 0x02,
            D = 0x03,
            DPlus = 0x04,
            CMinus = 0x05,
            C = 0x06,
            CPlus = 0x07,
            BMinus = 0x08,
            B = 0x09,
            BPlus = 0x0A,
            AMinus = 0x0B,
            A = 0x0C,
            APlus = 0x0D
        }



        /// <summary>
        /// Enumerates known Languages
        /// </summary>
        public enum Languages : byte
        {
            Unknown = 0x00,
            English = 0x01,
            English_uk = 0x02,
            French = 0x03,
            German = 0x04,
            Italian = 0x05,
            Spanish = 0x06,
            Dutch = 0x07,
            Danish = 0x08,
            Swedish = 0x09,
            Norwegian = 0x0a,
            Finnish = 0x0b,
            Hebrew = 0x0c,
            Russian = 0x0d,
            Portuguese = 0x0e,
            Japanese = 0x0f,
            Polish = 0x10,
            SimplifiedChinese = 0x11,
            TraditionalChinese = 0x12,
            Thai = 0x13,
            Korean = 0x14,
            Czech = 0x1a,
            Brazilian = 0x23
        }

        /// <summary>
        /// Enumerates available Datatypes
        /// </summary>
        public enum DataTypes : uint
        {
            dtUInteger = 0xEB61E4F7,
            dtString = 0x0B8BEA18,
            dtSingle = 0xABC78708,
            dtBoolean = 0xCBA908E1,
            dtInteger = 0x0C264712
        }

        /// <summary>
        /// Available Format Codes
        /// </summary>
        public enum FormatCode : ushort
        {
            normal = 0xFFFD,
        };

        /// <summary>
        /// Is an Item within the PackedFile Index new Alias(0x20 , "or 0x24 Bytes long"),
        /// </summary>
        public enum IndexTypes : uint
        {
            ptShortFileIndex = 0x01,
            ptLongFileIndex = 0x02
        }

        /// <summary>
        /// Which general apiration does a Sim have
        /// </summary>
        public enum AspirationTypes : ushort
        {
            Nothing = 0x00,
            Romance = 0x01,
            Family = 0x02,
            Fortune = 0x04,
            Reputation = 0x10,
            Knowledge = 0x20,
            Growup = 0x40,
            Fun = 0x80,
            Chees = 0x100
        }

        /// <summary>
        /// Relationships a Sim can have
        /// </summary>
        public enum RelationshipStateBits : byte
        {
            Crush = 0x00,
            Love = 0x01,
            Engaged = 0x02,
            Married = 0x03,
            Friends = 0x04,
            Buddies = 0x05,
            Steady = 0x06,
            Enemy = 0x07,
            Family = 0x0E,
            Known = 0x0F,
        }

        /// <summary>
        /// UIFlags2 - more relationship states
        /// </summary>
        public enum UIFlags2Names : byte
        {
            BestFriendForever = 0x00,
        };


        /// <summary>
        /// Available Zodia Signes
        /// </summary>
        public enum ZodiacSignes : ushort
        {
            Aries = 0x01,		 //de: Widder
            Taurus = 0x02,
            Gemini = 0x03,
            Cancer = 0x04,
            Leo = 0x05,
            Virgo = 0x06,		 //de: Jungfrau
            Libra = 0x07,		 //de: Waage
            Scorpio = 0x08,
            Sagittarius = 0x09,  //de: Sch�tze
            Capricorn = 0x0A,	 //de: Steinbock
            Aquarius = 0x0B,
            Pisces = 0x0C		 //de: Fische
        }

        /// <summary>
        /// Known Types for Family ties
        /// </summary>
        public enum FamilyTieTypes : uint
        {
            MyMotherIs = 0x00,
            MyFatherIs = 0x01,
            ImMarriedTo = 0x02,
            MySiblingIs = 0x03,
            MyChildIs = 0x04
        }

        /// <summary>
        /// Detailed Relationships between Sims
        /// </summary>
        public enum RelationshipTypes : uint
        {
            Unset_Unknown = 0x00,
            Parent = 0x01,
            Child = 0x02,
            Sibling = 0x03,
            Gradparent = 0x04,
            Grandchild = 0x05,
            Nice_Nephew = 0x07,
            Aunt = 0x06,
            Cousin = 0x08,
            Spouses = 0x09
        }

        /// <summary>
        /// How old (in Life Sections) is the Sim
        /// </summary>
        public enum LifeSections : ushort
        {
            Unknown = 0x00,
            Baby = 0x01,
            Toddler = 0x02,
            Child = 0x03,
            Teen = 0x10,
            Adult = 0x13,
            Elder = 0x33,
            YoungAdult = 0x40
        }

        /// <summary>
        /// Gender of a Sim
        /// </summary>
        public enum Gender : ushort
        {
            Male = 0x00,
            Female = 0x01
        }

        /// <summary>
        /// The Jobs known by SimPE
        /// </summary>
        /// <remarks>Use finder dock object search for JobData*</remarks>
        public enum Careers : uint
        {
            Unknown = 0xFFFFFFFF,
            Unemployed = 0x00000000,
            Military = 0x6C9EBD32,
            Politics = 0x2C945B14,
            Science = 0x0C9EBD47,
            Medical = 0x0C7761FD,
            Athletic = 0x2C89E95F,
            Economy = 0x45196555,
            LawEnforcement = 0xAC9EBCE3,
            Culinary = 0xEC9EBD5F,
            Slacker = 0xEC77620B,
            Criminal = 0x6C9EBD0E,
            TeenElderAthletic = 0xAC89E947,
            TeenElderBusiness = 0x4C1E0577,
            TeenElderCriminal = 0xACA07ACD,
            TeenElderCulinary = 0x4CA07B0C,
            TeenElderLawEnforcement = 0x6CA07B39,
            TeenElderMedical = 0xAC89E918,
            TeenElderMilitary = 0xCCA07B66,
            TeenElderPolitics = 0xCCA07B8D,
            TeenElderScience = 0xECA07BB0,
            TeenElderSlacker = 0x6CA07BDC,
            Paranormal = 0x2E6FFF87,
            NaturalScientist = 0xEE70001C,
            ShowBiz = 0xAE6FFFB0,
            Artist = 0x4E6FFFBC,
            Adventurer = 0x3240CBA5,
            Education = 0x72428B30,
            Gamer = 0xF240C306,
            Journalism = 0x7240D944,
            Law = 0x12428B19,
            Music = 0xB2428B0C,
            TeenElderAdventurer = 0xF240D235,
            TeenElderEducation = 0xD243BBEC,
            TeenElderGamer = 0x1240C962,
            TeenElderJournalism = 0x5240E212,
            TeenElderLaw = 0x1243BBDE,
            TeenElderMusic = 0xB243BBD2,
            PetSecurity = 0xD188A400,
            PetService = 0xB188A4C1,
            PetShowBiz = 0xD175CC2D,
            TeenElderConstruction = 0x53E1C30F,
            TeenElderDance = 0xD3E094A5,
            TeenElderEntertainment = 0x53E09494,
            TeenElderIntelligence = 0x93E094C0,
            TeenElderOcenography = 0x13E09443,
            Construction = 0xF3E1C301,
            Dance = 0xD3E09422,
            Entertainment = 0xB3E09417,
            Intelligence = 0x33E0940E,
            Ocenography = 0x73E09404

        }
        #endregion

        #region Arrays

        /// <summary>
        /// all Known SemiGlobal Groups
        /// </summary>
     
        static SemiGlobalListing sgl;
        public static System.Collections.Generic.List<SemiGlobalAlias> SemiGlobals{
             get {
                 if (sgl == null) LoadSemGlobList();
                 return sgl;
             }
        }
        static void LoadSemGlobList()
        {
            sgl = new SemiGlobalListing();
            sgl.Sort();
        }
        public static uint SemiGlobalID(string sgname)
        {
            foreach (SemiGlobalAlias sga in SemiGlobals) if (sga.Name.Trim().ToLowerInvariant().Equals(sgname.Trim().ToLowerInvariant())) return sga.Id;
            return 0;
        }
        public static string SemiGlobalName(uint sgid)
        {
            foreach (SemiGlobalAlias sga in SemiGlobals) if (sga.Id == sgid) return sga.Name;
            return "";
        }



        #endregion

        #region Supporting Methods
        /// <summary>
        /// Returns the describing TypeAlias for the passed Type
        /// </summary>
        /// <param name="type">The type you want to load the TypeAlias for</param>
        /// <returns>The TypeAlias representing the Type</returns>
        public static TypeAlias FindTypeAlias(UInt32 type)
        {
            Data.TypeAlias a = Helper.TGILoader.GetByType(type);
            if (a == null) a = new Data.TypeAlias(false, Localization.Manager.GetString("unk") + "", type, "0x" + Helper.HexString(type));
            return a;
        }

        /// <summary>
        /// Returns the Group Number of a SemiGlobal File
        /// </summary>
        /// <param name="name">the nme of the semi global</param>
        /// <returns>The group Vlue of the Global</returns>
        public static Alias FindSemiGlobal(string name)
        {
            name = name.ToLower();
            foreach (Alias a in Data.MetaData.SemiGlobals)
            {
                if (a.Name.ToLower() == name) return a;
            } //for

            //unknown SemiGlobal
            return new Alias(0xffffffff, name.ToLower());
        }
        #endregion

        #region Map's
        static ArrayList rcollist;
        static ArrayList complist;
        static Hashtable agelist;
        static System.Collections.Generic.List<uint> cachedft;

        public static System.Collections.Generic.List<uint> CachedFileTypes
        {
            get
            {
                if (cachedft == null)
                {
                    cachedft = new System.Collections.Generic.List<uint>();

                    foreach (uint i in RcolList)
                        cachedft.Add(i);

                    cachedft.Add(OBJD_FILE);
                    cachedft.Add(CTSS_FILE);
                    cachedft.Add(STRING_FILE);

                    cachedft.Add(XFLR);
                    cachedft.Add(XFNC);
                    cachedft.Add(XNGB);
                    cachedft.Add(XOBJ);
                    cachedft.Add(XROF);
                    cachedft.Add(XWNT);
                }
                return cachedft;
            }
        }

        //Returns a List of all RCOl Compatible File Types
        public static ArrayList RcolList
        {
            get
            {
                if (rcollist == null)
                {
                    rcollist = new ArrayList();

                    rcollist.Add((uint)GMDC);	//GMDC
                    rcollist.Add((uint)TXTR);	//TXTR
                    rcollist.Add((uint)LIFO);	//LIFO
                    rcollist.Add((uint)TXMT);	//MATD
                    rcollist.Add((uint)ANIM);	//ANIM
                    rcollist.Add((uint)GMND);	//GMND
                    rcollist.Add((uint)SHPE);	//SHPE
                    rcollist.Add((uint)CRES);	//CRES
                    rcollist.Add(LDIR);
                    rcollist.Add(LAMB);
                    rcollist.Add(LSPT);
                    rcollist.Add(LPNT);
                }

                return rcollist;
            }
        }

        //Returns a List of File Types that should be compressed
        public static ArrayList CompressionCandidates
        {
            get
            {
                if (complist == null)
                {
                    complist = RcolList;

                    complist.Add(MetaData.STRING_FILE);
                    complist.Add((uint)0x0C560F39); //Binary Index
                    complist.Add((uint)0xAC506764); //3D IDR
                }

                return complist;
            }
        }

        /// <summary>
        /// translates the Ages from a SDesc to a Property Set age 
        /// </summary>
        public static Data.Ages AgeTranslation(MetaData.LifeSections age)
        {
            agelist = new Hashtable();
            if (age == MetaData.LifeSections.Adult) return Data.Ages.Adult;
            else if (age == MetaData.LifeSections.Baby) return Data.Ages.Baby;
            else if (age == MetaData.LifeSections.Child) return Data.Ages.Child;
            else if (age == MetaData.LifeSections.Elder) return Data.Ages.Elder;
            else if (age == MetaData.LifeSections.Teen) return Data.Ages.Teen;
            else if (age == MetaData.LifeSections.Toddler) return Data.Ages.Toddler;
            else return Data.Ages.Adult;

        }
        #endregion
    }
}
