using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NSUci;
using NSChess;
using System.Windows.Forms;

namespace NSProgram
{
	static class Program
	{
		static void Main(string[] args)
		{
			CBookUmo Book = new CBookUmo();
			CUci Uci = new CUci();
			CChessExt Chess = new CChessExt();
			bool isWritable = false;
			bool bookRead = false;
			int bookLimitW = 0;
			int bookLimitR = 0;
			string ax = "-bn";
			List<string> movesUci = new List<string>();
			List<string> listBn = new List<string>();
			List<string> listEf = new List<string>();
			List<string> listEa = new List<string>();
			for (int n = 0; n < args.Length; n++)
			{
				string ac = args[n];
				switch (ac)
				{
					case "-bn":
					case "-ef":
					case "-ea":
					case "-lr":
					case "-lw":
						ax = ac;
						break;
					case "-w":
						ax = ac;
						isWritable = true;
						break;
					default:
						switch (ax)
						{
							case "-bn":
								listBn.Add(ac);
								break;
							case "-ef":
								listEf.Add(ac);
								break;
							case "-ea":
								listEa.Add(ac);
								break;
							case "-lr":
								bookLimitR = int.Parse(ac);
								break;
							case "-lw":
								bookLimitW = int.Parse(ac);
								break;
						}
						break;
				}
			}
			string bookName = String.Join(" ", listBn);
			string engineName = String.Join(" ", listEf);
			string arguments = String.Join(" ", listEa);
			Process myProcess = new Process();
			if (File.Exists(engineName))
			{
				myProcess.StartInfo.FileName = engineName;
				myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(engineName);
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.Arguments = arguments;
				myProcess.Start();
			}
			else
			{
				if (engineName != String.Empty)
					Console.WriteLine("info string missing engine");
				engineName = String.Empty;
			}
			if (!Book.Load(bookName))
				if (!Book.Load($"{bookName}{CBookUmo.defExt}"))
					if (!Book.Load($"{bookName}.uci"))
						Book.Load($"{bookName}.pgn");
			Console.WriteLine($"info string book {Book.moves.Count:N0} lines");
			while (true)
			{
				string msg = Console.ReadLine().Trim();
				if (String.IsNullOrEmpty(msg) || (msg == "help") || (msg == "book"))
				{
					Console.WriteLine("book load [filename].[umo|uci|png] - clear and add moves from file");
					Console.WriteLine("book save [filename].[umo|uci|png] - save book to the file");
					Console.WriteLine("book addfile [filename].[umo|uci|png] - add moves to the book from file");
					Console.WriteLine("book adduci [uci] - add moves in uci format to the book");
					Console.WriteLine("book clear - clear all moves from the book");
					continue;
				}
				Uci.SetMsg(msg);
				int count = Book.moves.Count;
				if (Uci.command == "book")
				{
					switch (Uci.tokens[1])
					{
						case "addfile":
							if (!Book.AddFile(Uci.GetValue(2, 0)))
								Console.WriteLine("File not found");
							else
								Console.WriteLine($"{(Book.moves.Count - count):N0} lines have been added");
							break;
						case "adduci":
							Book.moves.Add(Uci.GetValue(2, 0));
							Console.WriteLine($"{(Book.moves.Count - count):N0} lines have been added");
							break;
						case "clear":
							Book.Clear();
							Console.WriteLine("Book is empty");
							break;
						case "load":
							if (!Book.Load(Uci.GetValue(2, 0)))
								Console.WriteLine("File not found");
							else
								Console.WriteLine($"{Book.moves.Count:N0} lines in the book");
							break;
						case "save":
							Book.Save(Uci.GetValue(2, 0));
							Console.WriteLine("The book has been saved");
							break;
						default:
							Console.WriteLine($"Unknown command [{Uci.tokens[1]}]");
							break;
					}
					continue;
				}
				if ((Uci.command != "go") && (engineName != String.Empty))
					myProcess.StandardInput.WriteLine(msg);
				switch (Uci.command)
				{
					case "position":
						bookRead = false;
						movesUci.Clear();
						Chess.SetFen();
						if (Uci.GetIndex("fen") < 0)
						{
							bookRead = true;
							int m = Uci.GetIndex("moves", Uci.tokens.Length);
							for (int n = m + 1; n < Uci.tokens.Length; n++)
							{
								string umo = Uci.tokens[n];
								movesUci.Add(umo);
								int emo = Chess.UmoToEmo(umo);
								Chess.MakeMove(emo);
							}
							if (isWritable && Chess.Is2ToEnd(out string mm, out string em))
							{
								movesUci.Add(mm);
								movesUci.Add(em);
								int c = bookLimitW > 0 ? bookLimitW : movesUci.Count;
								List<string> l = movesUci.GetRange(0, c);
								string lm = String.Join(" ", l);
								Book.moves.Add(lm);
								Book.Save();
							}
						}
						break;
					case "go":
						string move = String.Empty;
						if (bookRead && ((movesUci.Count < bookLimitR) || (bookLimitR == 0)))
						{
							string moves = String.Join(" ", movesUci);
							move = Book.GetMove(moves);
						}
						if (move != String.Empty)
						{
							Console.WriteLine("info string book");
							Console.WriteLine($"bestmove {move}");
						}
						else if (engineName == String.Empty)
							Console.WriteLine("enginemove");
						else
							myProcess.StandardInput.WriteLine(msg);
						break;
				}
			}
		}
	}
}
