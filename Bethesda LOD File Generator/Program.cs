using System;
using System.IO;

namespace Bethesda_LOD_File_Generator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				ShowUsageText();
				return;
			}
			else if (args.Length == 1)
			{
				switch (Path.GetExtension(args[0]).ToUpper())
				{
					case ".TXT":
						{
							Converter.ConvertToLOD(args[0]);
							return;
						}
					case ".LOD":
						{
							Converter.ConvertFromLOD(args[0]);
							return;
						}

					default:
						{
							Console.WriteLine("ERROR");
							Console.WriteLine("File is not a .lod or .txt file.");
							Console.WriteLine();
							ShowUsageText();
							return;
						}
				}
			}
			else if (args.Length > 1) // Handle multiple files at once
			{
				foreach (var arg in args)
				{
					switch (Path.GetExtension(arg))
					{
						case ".txt":
							{
								Converter.ConvertToLOD(arg, false);
								break;
							}
						case ".lod":
							{
								Converter.ConvertFromLOD(arg, false);
								break;
							}

						// Skip file
						default:
							{
								break;
							}
					}
				}
			}
		}

		// Usage Text
		static void ShowUsageText()
		{
			Console.WriteLine("USAGE");
			Console.WriteLine("Drag and drop a text file (*.txt) in the format shown below onto the exe.");
			Console.WriteLine();
			Console.WriteLine("FORMAT");
			Console.WriteLine("Worldspace: MyWorldspaceID");
			Console.WriteLine("NW Corner: X, Y");
			Console.WriteLine("SE Corner: X, Y");
			Console.WriteLine("Lowest LOD: 4, 8, 16, or 32 (Optional; Recommended: 4; Default: 4)");
			Console.WriteLine("Highest LOD: 4, 8, 16, or 32 (Optional; Default: 32)");
			Console.WriteLine();
			GenerateTemplateFile();
		}

		// Generate a template file if the user wishes
		static void GenerateTemplateFile()
		{
			if (Confirm("Generate template file?"))
			{
				string fileName = "Template.txt";

				using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create)))
				{
					sw.WriteLine("Worldspace: MyWorldspace");
					sw.WriteLine("NW Corner: -16, 16");
					sw.WriteLine("SE Corner: 16, -16");
					sw.WriteLine("Lowest LOD: 4, 8, 16, or 32 (Optional; Recommended: 4; Default: 4)");
					sw.WriteLine("Highest LOD: 4, 8, 16, or 32 (Optional; Default: 32)");
				}
			}
		}

		static bool Confirm(string title)
		{
			ConsoleKey response;

			do
			{
				Console.Write($"{title} [Y/N]: ");
				response = Console.ReadKey(false).Key;

				if (response != ConsoleKey.Enter)
					Console.WriteLine();
			}
			while (response != ConsoleKey.Y && response != ConsoleKey.N);

			return response == ConsoleKey.Y;
		}
	}
}
