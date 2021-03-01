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
				CBookUmo Book = new CBookUmo();
				CUci Uci = new CUci();
				CChess Chess = new CChess();
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
				string book = String.Join(" ", listBn);
				string engine = String.Join(" ", listEf);
				string arguments = String.Join(" ", listEa);
				if (book == "")
					book = Path.GetFileNameWithoutExtension(engine);
				Process myProcess = new Process();
				if (File.Exists(engine))
				{
					myProcess.StartInfo.FileName = engine;
					myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(engine);
					myProcess.StartInfo.UseShellExecute = false;
					myProcess.StartInfo.RedirectStandardInput = true;
					myProcess.StartInfo.Arguments = arguments;
					myProcess.Start();
				}
				else
				{
					if (engine != "")
						Console.WriteLine("info string missing engine");
					engine = "";
				}
				if (!Book.Load(book))
					if (!Book.Load($"{book}{CBookUmo.defExt}"))
						if (!Book.Load($"{book}.uci"))
							if (!Book.Load($"{book}.pgn"))
								Console.WriteLine($"info string missing book {book}");
				while (true)
				{
					string msg = Console.ReadLine();
					Uci.SetMsg(msg);
					if ((Uci.command != "go") && (engine != ""))
						myProcess.StandardInput.WriteLine(msg);
					switch (Uci.command)
					{
						case "position":
							bookRead = false;
							movesUci.Clear();
							Chess.SetFen("");
							if (Uci.GetIndex("fen", 0) == 0)
							{
								bookRead = true;
								int m = Uci.GetIndex("moves", Uci.tokens.Length);
								for (int n = m; n < Uci.tokens.Length; n++)
								{
									string umo = Uci.tokens[n];
									movesUci.Add(umo);
									int emo = Chess.UmoToEmo(umo);
									Chess.MakeMove(emo);
								}
								if ((bookLimitW > 0) && Chess.Is2ToEnd(out string mm, out string em))
								{
									movesUci.Add(mm);
									movesUci.Add(em);
									List<string> l = movesUci.GetRange(0, bookLimitW);
									string lm = String.Join(" ", l);
									Book.moves.Add(lm);
									Book.Save(book);
								}
							}
							break;
						case "go":
							string move = "";
							if (bookRead && ((movesUci.Count < bookLimitR) || (bookLimitR == 0)))
							{
								string moves = String.Join(" ", movesUci);
								move = Book.GetMove(moves);
							}
							if (move != "")
							{
								Console.WriteLine("info string book");
								Console.WriteLine($"bestmove {move}");
							}
							else if (engine == "")
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
