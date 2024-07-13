using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Bethesda_LOD_File_Generator
{
	internal partial class Converter
	{
		// List of existing worldspace names in the Skyrim and Creation Club ESM/ESL files
		static List<string> SkyrimWorldspaces = new List<string>() { "Blackreach", "ccBGSSSE067DeadlandsWorld", "ccKRTSSE001QNWorld", "DeepwoodRedoubtWorld",
																	 "DLC01FalmerValley", "DLC01SoulCairn", "DLC1HunterHQWorld", "DLC2ApocryphaWorld",
																	 "DLC2SolstheimWorld", "JaphetsFollyWorld", "MarkarthWorld", "SkuldafnWorld", "Sovngarde", "Tamriel" };

		// List of existing worldspace names in the Fallout 4/76 and Creation Club ESM/ESL files
		static List<string> FalloutWorldspaces = new List<string>() { "Appalachia", "ccBGSFO4040VRWorkshopGridWorld", "ccBGSFO4117GNRPlaza", "ccFSVFO4004VRWorkshopGNRPlaza",
																	  "ccFSVFO4005VRWorkshopDesertIsland", "ccFSVFO4006VRWorkshopWasteland", "Commonwealth", "DiamondCity",
																	  "DLC03FarHarbor", "DLC03VRWorldspace", "NukaWorld", "SanctuaryHillsWorld" };

		// List of existing worldspace names in the Starfield ESM/ESL files
		static List<string> StarfieldWorldspaces = new List<string>() { "akilacity", "CydoniaCity", "DefaultSpaceshipLandingOverlay", "DefaultWorld", "DR001World", "DR002World",
																		"DR003CaveWorldD", "DR006World03", "DR007World", "DR008World", "DR009World", "DR010World", "DR011World",
																		"DR012World", "DR014GarageWorldB", "DR015World", "DR016World", "DR017World", "DR018World01", "DR020WorldNew",
																		"DR021World", "DR022World", "DR023World", "DR024World", "DR025World", "DR026World", "DR027World", "DR029World",
																		"DR034World", "DR035World", "LandingOverlay01", "LandingOverlay02", "LandingOverlay03", "LandingOverlay04",
																		"LandingOverlay05", "lc001world", "LC003KreetBase", "LC006World", "lc017world", "LC024World", "LC028AbandonedMines",
																		"LC028DrillExit", "LC028FreightElevatorExit", "LC030World", "LC032LondinionBWorld", "LC042BattleofNiira", "LC062TauGourmetWorld",
																		"lc070world01", "LC071World", "LC074World", "LC092World", "LC093KazaalSulfurMineWorldB", "lc116world", "LC119TheMantisLairWorld",
																		"lc123world", "LC124World", "LC125World", "lc126world", "LC127World", "LC131World", "LC132World", "lc133world", "LC134World",
																		"LC135World", "lc136world", "LC139World", "LC142World", "LC143World", "LC144World", "LC146World", "LC148World", "LC151World",
																		"LC152World", "LC153World", "LC154World", "LC155World", "LC159World", "lc160world", "LC161World", "LC164World", "LC165World",
																		"LC167World", "LC171World", "LC172World", "LC173World", "LC174World", "LC176World", "LC178World", "LC179World", "LC180World",
																		"NeonCity", "NewAtlantis", "OEAF001CaveWorld", "OEAF002CaveWorld", "OEAF003CaveWorld", "OEAF004CaveWorld", "OEAF005CaveWorld",
																		"OEAF006CaveWorld", "OEAF007CaveWorld", "OEAF008CaveWorld", "OEAF009CaveWorld", "OEAF010CaveWorld", "OEAF011CaveWorld", "OEAF014CaveWorld",
																		"OEAF015World", "OEAF016World", "OEAF017World", "OEAF018World", "OEAF019World", "OEAF020World", "OEAF021World", "OEAF022World", "OEAF023World",
																		"OEAF024World", "OEAF025World", "OEAF026World", "OEAH001World", "OEAH002World", "OEBB001World", "OEBB002World", "OEBB003World", "OEBB004World",
																		"OEBB005World", "OEBB007World", "OEBB008World", "OEBB009World", "OEBB010World", "OEBB011World", "OEBB012World", "OEBB013World", "OEBB014World",
																		"OEBB015World", "OEBB016World", "OEBB017World", "OEBB018World", "OEBB019World", "OEBB020World", "OEBB021World", "OEBB022World", "OEBB023World",
																		"OEBB024World", "OEBB026World", "OEBB027World", "OEBB028World", "OEBB029World", "OEBB030World", "OEBB031World", "OEBB032World", "OEBB034World",
																		"OEBB035World", "OEDB001World", "OEDB002World", "OEDB502World", "OEDB503World", "OEDB505World", "OEDB506World", "OEDB507World", "OEDB508World",
																		"OEDB509World", "OEDB510World", "OEDB511World", "oedebugoverlay", "OEJM001World", "OEJM002World", "OEJM003World", "OEJM004World", "OEJM005World",
																		"OEJM006World", "OEJM007World", "OEJM008World", "OEJM009World", "OEJM010World", "OEJM011World", "OEJM012World", "OEJM013World", "OEJM014World",
																		"OEJM015World", "OEJP001CaveWorld", "OEJP002World", "OEJP004CaveWorld", "OEJP005CaveWorld", "OEJP006CaveWorld", "OEJP008CaveWorld", "OEJP017World",
																		"OEJP018World", "OEJP019World", "OEJP020World", "OEJP021World", "OEJP022World", "OEJP023World", "OEJP024World", "OEJP025World", "OEJP026World",
																		"OEJP027World", "OEJP029World", "OEJP030World", "OEJP031World", "OEJP032World", "OEJP033World", "OEKMK002SupplyMissionWorld", "OEOB001World",
																		"OEOB002CaveWorld", "OEOB003CaveWorld", "OEOB004CaveWorld", "OEOB005CaveWorld", "OEOB006World", "OEOB007World", "OEOB008World", "OESC001World",
																		"OESC002World", "OESC003World", "OESD001CaveWorld", "OESD002World", "OESD003CaveWorld", "OESD004World", "OESD005World", "OESD006World", "OESD007CaveWorld",
																		"OESD008World", "OESD009World", "OESD010World", "OESD011World", "OESD012World", "OESD013World", "OESD014World", "OESD015World", "OESD016World",
																		"OESD017World", "OESD018World", "OESF001World", "OESF002World", "OESF003World", "OESF004World", "OESF005World", "OESF006World", "OESF007World",
																		"OESF008World", "OESF010World", "OEZW001CaveWorld", "OEZW002CaveWorld", "OEZW003CaveWorld", "OEZW004CaveWorld", "OEZW006CaveWorld", "OEZW007CaveWorld",
																		"OEZW008CaveWorld", "OEZW009World", "OutpostOverlayTemplate", "OverlayBlockPOIBiomeOasis01", "OverlayPOIBiomeArch01", "OverlayPOIBiomeCraterGrove01",
																		"OverlayPOIBiomeCrystalCave1", "OverlayPOIBiomeCrystalTrees01", "OverlayPOIBiomeDenLayered01", "OverlayPOIBiomeDenLayered02", "OverlayPOIBiomeDenLayered03",
																		"OverlayPOIBiomeDenPocked01", "OverlayPOIBiomeDenPocked02", "OverlayPOIBiomeDenRough01", "OverlayPOIBiomeDenRough02", "OverlayPOIBiomeDenSharp01",
																		"OverlayPOIBiomeDenSharp02", "OverlayPOIBiomeDenSharp03", "OverlayPOIBiomeDenVeiny01", "OverlayPOIBiomeDenVeiny02", "OverlayPOIBiomeDenVolcanicRough01",
																		"OverlayPOIBiomeDenVolcanicRough02", "OverlayPOIBiomeDenVolcanicWorn01", "OverlayPOIBiomeDenVolcanicWorn02", "OverlayPOIBiomeFairyRing01",
																		"OverlayPOIBiomeFloraGrove01", "OverlayPOIBiomeOasisLayered01", "OverlayPOIBiomeOasisLayered02", "OverlayPOIBiomeOasisPocked01",
																		"OverlayPOIBiomeOasisPocked02", "OverlayPOIBiomeOasisRough01", "OverlayPOIBiomeOasisRough02", "OverlayPOIBiomeOasisSharp01",
																		"OverlayPOIBiomeOasisSharp02", "OverlayPOIBiomeOasisVeiny01", "OverlayPOIBiomeOasisVeiny02", "OverlayPOIBiomeOasisVolcanicRough01",
																		"OverlayPOIBiomeOasisVolcanicRough02", "OverlayPOIBiomeOasisVolcanicWorn01", "OverlayPOIBiomeOasisVolcanicWorn02", "OverlayPOIBiomeStrangeValley01",
																		"OverlayPOIBiomeValleyofTrees01", "OverlayTraitAeriformLifeMed01", "OverlayTraitAeriformLifeSm01", "OverlayTraitAeriformLifeSm02",
																		"OverlayTraitAmphibiousFootholdMed01", "OverlayTraitAmphibiousFootholdMed02", "OverlayTraitAmphibiousFootholdSm01", "OverlayTraitBoiledSeasMed01",
																		"OverlayTraitBoiledSeasMed02", "OverlayTraitBoiledSeasSm01new", "OverlayTraitBolideBombardmentMed01", "OverlayTraitBolideBombardmentMed01Life",
																		"OverlayTraitBolideBombardmentMed02", "OverlayTraitBolideBombardmentMed02Life", "OverlayTraitBolideBombardmentMed03",
																		"OverlayTraitBolideBombardmentMed03Life", "OverlayTraitCharredEcosystemMd01", "OverlayTraitCharredEcosystemMd01Water", "OverlayTraitCharredEcosystemMd02",
																		"OverlayTraitCharredEcosystemMd02Water", "OverlayTraitCharredEcosystemSm01", "OverlayTraitCharredEcosystemSm01Water", "OverlayTraitContinualConducterMd01",
																		"OverlayTraitContinualConductorSm02", "OverlayTraitContualConductorSm01", "OverlayTraitCorallineLandmassMed01", "OverlayTraitCorallineLandmassMed02",
																		"OverlayTraitCorallineLandmassMed03", "OverlayTraitCrystallineCrustLg01", "OverlayTraitCrystallineCrustLg02", "OverlayTraitCrystallineCrustSm01",
																		"OverlayTraitDiseasedBiosphereMed01", "OverlayTraitDiseasedBiosphereMed02", "OverlayTraitDiseasedBiosphereSm01", "OverlayTraitEcologicalConsortiumMd01",
																		"OverlayTraitEcologicalConsortiumSm01", "OverlayTraitEcologicalConsortiumSm02", "OverlayTraitEmergingTectonicsMed01", "OverlayTraitEmergingTectonicsMed02",
																		"OverlayTraitEmergingTectonicsSm01", "OverlayTraitEnergeticRiftingMd02", "OverlayTraitEnergeticRiftingSm01", "OverlayTraitEnergeticRiftingSm04",
																		"OverlayTraitExtinctionEventMed01", "OverlayTraitExtinctionEventMed02", "OverlayTraitExtinctionEventSm01", "OverlayTraitFrozenEcosystemMed01",
																		"OverlayTraitFrozenEcosystemSm01", "OverlayTraitFrozenEcosystemSm02", "OverlayTraitGaseousFontMed01", "OverlayTraitGaseousFontMed02",
																		"OverlayTraitGaseousFontMed03new", "OverlayTraitGlobalGlacialRecessionMed01", "OverlayTraitGlobalGlacialRecessionSm01",
																		"OverlayTraitGlobalGlacialRecessionSm02", "OverlayTraitGravitationalAnomalySm01", "OverlayTraitGravitationalAnomalySm02",
																		"OverlayTraitGravitationalAnomalySm03", "OverlayTraitPeltedFieldMed01", "OverlayTraitPeltedFieldSm01", "OverlayTraitPeltedFieldSm02",
																		"OverlayTraitPrimedLifeMd01", "OverlayTraitPrimedLifeMd02", "OverlayTraitPrimedLifeSm01", "OverlayTraitPrimordialNetworkMed01",
																		"OverlayTraitPrimordialNetworkMed02new", "OverlayTraitPrimordialNetworkSm01", "OverlayTraitPrismaticPlumesMd01", "OverlayTraitPrismaticPlumesSm01",
																		"OverlayTraitPrismaticPlumesSm02", "OverlayTraitPsychotropicBiotaMed01new", "OverlayTraitPsychotropicBiotaSm01", "OverlayTraitPsychotropicBiotaSm02",
																		"OverlayTraitSentientMicrobialColoniesLg01new", "OverlayTraitSentientMicrobialColoniesMed01", "OverlayTraitSentientMicrobialColoniesMed02",
																		"OverlayTraitSlushySubsurfaceSeasMed01", "OverlayTraitSlushySubsurfaceSeasSm01", "OverlayTraitSlushySubsurfaceSeasSm02",
																		"OverlayTraitSolarStormsSeasonsLg01Life", "OverlayTraitSolarStormsSeasonsLg01new", "OverlayTraitSolarStormsSeasonsMed01",
																		"OverlayTraitSolarStormsSeasonsMed01Life", "OverlayTraitSolarStormsSeasonsMed02", "OverlayTraitSolarStormsSeasonsMed02Life",
																		"OverlayTraitSonorousLilthosphereMed02", "OverlayTraitSonorousLithosphereMed01", "OverlayTraitSonorousLithosphereSm01",
																		"OverlayTraitTurbulentLithosphereMed01", "OverlayTraitTurbulentLithosphereSm01", "OverlayTraitTurbulentLithosphereSm02", "PCMGenericLandingOverlay",
																		"PCMGenericLandingUndiscoveredOverlay", "PCMOverlayAnimalDenSmallCanyon01", "PCMOverlayREShip01", "PCMShipLanding01", "PCMShipLanding02",
																		"PCMShipLanding03", "PCMShipLanding04", "POI001World", "POIJP003World", "POIJP007World", "POIJP009World", "POIJP010World", "POIJP011World",
																		"POIJP012World", "POIJP013World", "POIJP015World", "POIJP016World", "RL003World", "RL005MuybridgeWorld", "RL010World", "RL012World", "RL018Worlda",
																		"RL032World", "RL036World", "RL037World", "RL039World", "RL040World", "RL064World", "RL067World", "RL089WorldNEW", "RL090World", "SB001TempleWorld",
																		"SB002TempleWorld", "SB003TempleWorld02", "SB004TempleWorld", "SB005TempleWorld", "SB006TempleWorld", "SB007TempleWorld", "SB008TempleWorld",
																		"SB009TempleWorld", "SB010TempleWorld", "SB011TempleWorld", "SB012TempleWorld", "SB013TempleWorld", "SB014TempleWorld", "SB015TempleWorld",
																		"SB016TempleWorld", "SettleCrucibleWorld", "settlegagarinworld", "settlehopetownworld", "SettleNewHomesteadWorld", "settleparadisoworld", "settleredmile",
																		"settletheeleosretreatworld", "SettleWaggonerFarm", "SFTA01CaveHideout", "SFTA01HughesRanchWorld", "SFTA01SumatiCampWorld", "TempJemisonWorld" };

		public static void ConvertFromLOD(string file, bool doLogging = true)
		{
			string worldID = Path.GetFileNameWithoutExtension(file);
			short x = 0;
			short y = 0;
			int size = 0;
			int lowLOD = 0;
			int highLOD = 0;

			// Correctly capitalize worldspace names for known worldspaces
			worldID = CheckForKnownWorldspace(worldID);

			if (doLogging)
				Console.WriteLine("MODE - CONVERT FROM LOD");

			#region Read Input
			using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
			{
				Byte[] data = br.ReadBytes(16);
				x = BitConverter.ToInt16(data, 0);
				y = BitConverter.ToInt16(data, 2);
				size = BitConverter.ToInt32(data, 4);
				lowLOD = BitConverter.ToInt32(data, 8);
				highLOD = BitConverter.ToInt32(data, 12);
			}

			if (doLogging)
			{
				Console.WriteLine();
				Console.WriteLine($"Worldspace: {worldID}");
				Console.WriteLine($"West: {x}");
				Console.WriteLine($"South: {y}");
				Console.WriteLine($"Width/Height: {size}");
				Console.WriteLine($"Lowest LOD: {lowLOD}");
				Console.WriteLine($"Highest LOD: {highLOD}");
				Console.WriteLine();
			}
			#endregion

			#region Write Output
			string fileName = worldID + ".txt";

			using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create)))
			{
				sw.WriteLine($"Worldspace: {worldID}");
				sw.WriteLine($"West: {x}");
				sw.WriteLine($"South: {y}");
				sw.WriteLine($"Width/Height: {size}");
				sw.WriteLine($"Lowest LOD: {lowLOD}");
				sw.WriteLine($"Highest LOD: {highLOD}");
			}

			if (doLogging)
			{
				Console.WriteLine("Done!");
				Console.ReadLine();
			}
			#endregion
		}

		private static List<List<string>> knownWorldLists = new List<List<string>> { SkyrimWorldspaces, FalloutWorldspaces, StarfieldWorldspaces };
		static string CheckForKnownWorldspace(string str)
		{
			foreach (List<string> list in knownWorldLists)
			{
				if (list.Contains(str, StringComparer.OrdinalIgnoreCase))
				{
					int knownWorldIndex = list.FindIndex(i => i.Equals(str, StringComparison.OrdinalIgnoreCase));
					return list[knownWorldIndex];
				}
			}

			return str;
		}
	}
}