using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Bethesda_LOD_File_Generator
{
	internal partial class Converter
	{
		static readonly int lodV1_Len = 16;
		static readonly int lodV2_Len = 20;

		public static void ConvertFromLOD(string file, bool doLogging = true)
		{
			string worldID = Path.GetFileNameWithoutExtension(file);
			var x = 0;
			var y = 0;
			int size = 0;
			int lowLOD = 0;
			int highLOD = 0;
			bool lodV2File = false;

			// Correctly capitalize worldspace names for known worldspaces
			worldID = CheckForKnownWorldspace(worldID);

			if (doLogging)
				Console.WriteLine("MODE - CONVERT FROM LOD");

			#region Read Input
			using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
			{
				int fileSize = (int)br.BaseStream.Length;

				if (fileSize == lodV1_Len)
				{
					x = br.ReadInt16();
					y = br.ReadInt16();
					size = br.ReadInt32();
					lowLOD = br.ReadInt32();
					highLOD = br.ReadInt32();
				}
				else if (fileSize == lodV2_Len)
				{
					x = br.ReadInt32();
					y = br.ReadInt32();
					size = br.ReadInt32();
					lowLOD = br.ReadInt32();
					highLOD = br.ReadInt32();
					lodV2File = true;
				}
				else
				{
					Console.WriteLine("ERROR ON INPUT FILE");
					Console.WriteLine(worldID);
					Console.WriteLine("----------------------------------------------------------------------------------------");
					Console.WriteLine("File is not a valid Creation Engine 1 or 2 LOD file.");
					Console.WriteLine("Maybe you're trying to convert an unused (and therefore unsupported) Starfield LOD file?");
					Console.ReadLine();
					return;
				}
			}

			if (doLogging)
			{
				Console.WriteLine();
				Console.WriteLine($"Worldspace: {worldID}");
				if (lodV2File)
				{
					Console.WriteLine($"objBound Min X: {x}");
					Console.WriteLine($"objBound Min Y: {y}");
					Console.WriteLine($"Size: {size}");
					Console.WriteLine($"Unk1: {lowLOD}");
					Console.WriteLine($"Unk2: {highLOD}");
				}
				else
				{
					Console.WriteLine($"West: {x}");
					Console.WriteLine($"South: {y}");
					Console.WriteLine($"Width/Height: {size}");
					Console.WriteLine($"Lowest LOD: {lowLOD}");
					Console.WriteLine($"Highest LOD: {highLOD}");
				}
				Console.WriteLine();
			}
			#endregion

			#region Write Output
			string fileName = worldID + ".txt";

			using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create)))
			{
				sw.WriteLine($"Worldspace: {worldID}");
				if (lodV2File)
				{
					sw.WriteLine($"objBound Min X: {x}");
					sw.WriteLine($"objBound Min Y: {y}");
					sw.WriteLine($"Size: {size}");
					sw.WriteLine($"Unk1: {lowLOD}");
					sw.WriteLine($"Unk2: {highLOD}");
				}
				else
				{
					sw.WriteLine($"West: {x}");
					sw.WriteLine($"South: {y}");
					sw.WriteLine($"Width/Height: {size}");
					sw.WriteLine($"Lowest LOD: {lowLOD}");
					sw.WriteLine($"Highest LOD: {highLOD}");
				}
			}

			if (doLogging)
			{
				Console.WriteLine("Done!");
				Console.ReadLine();
			}
			#endregion
		}

		private static List<List<string>> knownWorldLists = new List<List<string>> { Worldspaces.Skyrim, Worldspaces.Fallout, Worldspaces.Starfield };
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