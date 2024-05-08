using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bethesda_LOD_File_Generator
{
	internal partial class Converter
	{
		public static void ConvertToLOD(string file, bool doLogging = true)
		{
			string worldID = "Default";
			short x = 0;
			short y = 0;
			int size = 0;
			int lowLOD = 4;
			int highLOD = 32;

			if (doLogging)
				Console.WriteLine("MODE - CONVERT TO LOD");

			#region Read Input
			using (StreamReader sr = new StreamReader(file))
			{
				short NW_X = 0;
				short NW_Y = 0;
				short SE_X = 0;
				short SE_Y = 0;
				int LOD1 = 0;
				int LOD2 = 0;

				while (!sr.EndOfStream)
				{
					var line = sr.ReadLine();

					if (line != null)
					{
						if (StringIsCorrectlyFormatted(line, StringFormat.WorldID))
						{
							worldID = line.Remove(0, 12);
						}
						else if (StringIsCorrectlyFormatted(line, StringFormat.NWCorner))
						{
							string isolated = line.Remove(0, 11);
							string[] split = isolated.Split(',');
							NW_X = short.Parse(split[0]);
							NW_Y = short.Parse(split[1]);
						}
						else if (StringIsCorrectlyFormatted(line, StringFormat.SECorner))
						{
							string isolated = line.Remove(0, 11);
							string[] split = isolated.Split(',');
							SE_X = short.Parse(split[0]);
							SE_Y = short.Parse(split[1]);
						}
						else if (StringIsCorrectlyFormatted(line, StringFormat.LowLOD))
						{
							LOD1 = Int32.Parse(line.Remove(0, 12));
						}
						else if (StringIsCorrectlyFormatted(line, StringFormat.HighLOD))
						{
							LOD2 = Int32.Parse(line.Remove(0, 13));
						}
					}
				}

				x = Math.Min(NW_X, SE_X);
				y = Math.Min(NW_Y, SE_Y);

				int pow2 = ToNearest(Math.Abs(NW_X - SE_X));
				if (pow2 < 4)
					pow2 = 4;
				else if (pow2 > 256)
					pow2 = 256;

				size = pow2;

				if (LOD1 != 0)
					lowLOD = LOD1;
				if (LOD2 != 0)
					highLOD = LOD2;

				// Swap low and high LOD values if lowLOD is greater than highLOD
				if (lowLOD > highLOD)
					(lowLOD, highLOD) = (highLOD, lowLOD);
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
			string fileName = worldID + ".lod";
			List<Byte[]> data = BuildByteStream(x, y, size, lowLOD, highLOD);

			using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
			{
				bw.Write(data[0]);
				bw.Write(data[1]);
				bw.Write(data[2]);
				bw.Write(data[3]);
				bw.Write(data[4]);
			}

			if (doLogging)
			{
				Console.WriteLine("Done!");
				Console.ReadLine();
			}
			#endregion
		}

		// The .lod file stream
		static List<Byte[]> BuildByteStream(short x, short y, int size, int lowLOD, int highLOD)
		{
			List<Byte[]> data = new List<Byte[]> { BitConverter.GetBytes(x), BitConverter.GetBytes(y), BitConverter.GetBytes(size), BitConverter.GetBytes(lowLOD), BitConverter.GetBytes(highLOD) };

			return data;
		}

		enum StringFormat
		{
			WorldID,
			NWCorner,
			SECorner,
			LowLOD,
			HighLOD
		}

		// Check string to make sure it's formatted correctly
		static bool StringIsCorrectlyFormatted(string line, StringFormat format)
		{
			Regex worldID = new Regex(@"Worldspace: [\w\d]+$");
			Regex nwCorner = new Regex(@"NW Corner: -?(\d|[1-9][0-9]|1[01][0-9]|12[0-8]), -?(\d|[1-9][0-9]|1[01][0-9]|12[0-8])$");
			Regex seCorner = new Regex(@"SE Corner: -?(\d|[1-9][0-9]|1[01][0-9]|12[0-8]), -?(\d|[1-9][0-9]|1[01][0-9]|12[0-8])$");
			Regex lowLOD = new Regex(@"Lowest LOD: (4|8|1[6]|3[2])$");
			Regex highLOD = new Regex(@"Highest LOD: (4|8|1[6]|3[2])$");

			switch (format)
			{
				case StringFormat.WorldID:
					{
						return worldID.Match(line).Success;
					}
				case StringFormat.NWCorner:
					{
						return nwCorner.Match(line).Success;
					}
				case StringFormat.SECorner:
					{
						return seCorner.Match(line).Success;
					}
				case StringFormat.LowLOD:
					{
						return lowLOD.Match(line).Success;
					}
				case StringFormat.HighLOD:
					{
						return highLOD.Match(line).Success;
					}

				// We somehow supplied an incorrect StringFormat value
				default:
					{
						Console.WriteLine("Don't know what happened... please restart the program :(");
						Console.ReadLine();
						return false;
					}
			}
		}

		static int ToNextNearest(int x)
		{
			if (x < 0)
				return 0;

			--x;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;

			return x + 1;
		}

		static int ToNearest(int x)
		{
			int next = ToNextNearest(x);
			int prev = next >> 1;

			return next - x <= x - prev ? next : prev;
		}
	}
}