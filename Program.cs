using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NSUci;
using NSChess;
using RapBookUci;
using System.Windows.Forms;

namespace NSProgram
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FormBook());
			}
			else
			{
				CBookUmo book = new CBookUmo();
				CUci Uci = new CUci();
				CChess Chess = new CChess();
				bool isWritable = false;
				bool bookRead = false;
				int bookLimitW = 0xf;
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
				if (!book.Load(bookName))
					if (!book.Load($"{bookName}{CBookUmo.defExt}"))
						if (!book.Load($"{bookName}.uci"))
							if (!book.Load($"{bookName}.pgn"))
								Console.WriteLine($"info string missing book {bookName}");
				while (true)
				{
					string msg = Console.ReadLine();
					Uci.SetMsg(msg);
					if (Uci.command == "help")
					{
						Console.WriteLine("book addfile [filename].[umo|uci|png] - add moves to the book");
						Console.WriteLine("book save [filename].[umo|uci|png] - save book to the file");
						Console.WriteLine("book clear - clear all moves from the book");
						continue;
					}
					if (Uci.command == "book")
					{
						if (Uci.tokens.Length > 1)
							switch (Uci.tokens[1])
							{
								case "clear":
									book.Clear();
									break;
								case "addfile":
									if (!book.FileAdd(Uci.GetValue(2, 0)))
										Console.WriteLine("File not found");
									break;
								case "adduci":
									book.moves.Add(Uci.GetValue(2, 0));
									break;
								case "save":
									book.Save(Uci.GetValue(2, 0));
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
									int count = bookLimitW > 0 ? bookLimitW : movesUci.Count;
									List<string> l = movesUci.GetRange(0, count);
									string lm = String.Join(" ", l);
									book.moves.Add(lm);
									book.Save();
								}
							}
							break;
						case "go":
							string move = String.Empty;
							if (bookRead && ((movesUci.Count < bookLimitR) || (bookLimitR == 0)))
							{
								string moves = String.Join(" ", movesUci);
								move = book.GetMove(moves);
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
}
