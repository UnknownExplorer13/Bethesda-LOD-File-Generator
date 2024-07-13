using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Bethesda_LOD_File_Generator
{
	internal partial class Converter
	{
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