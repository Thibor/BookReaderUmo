using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NSChess;

namespace NSProgram
{
	public class CBookUmo
	{
		public const string defExt = ".umo";
		string path = $"Book{defExt}";
		readonly string name = "BookReaderUmo";
		readonly string version = "2020-12-01";
		public List<string> moves = new List<string>();
		readonly CChessExt Chess = new CChessExt();

		void ShowCountLines()
		{
			Console.WriteLine($"info string book {moves.Count:N0} lines");
		}

		public void Clear()
		{
			moves.Clear();
			ShowCountLines();
		}

		public string GetMove(string m)
		{
			if (moves.Count < 1)
				return String.Empty;
			int indexL = 0;
			int indexH = moves.Count - 1;
			m = m.Trim();
			if (m.Length > 0)
			{
				while (moves[indexL].IndexOf(m) != 0)
				{
					indexL++;
					if (indexL > indexH)
						return String.Empty;
				}
				while (moves[indexH].IndexOf(m) != 0)
				{
					indexH--;
					if (indexL > indexH)
						return "";
				}
			}
			int index = CChess.random.Next(indexL, indexH + 1);
			string[] mo = m.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] mr = moves[index].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (mr.Length > mo.Length)
				return mr[mo.Length];
			return "";
		}

		void DeleteDuplicates(List<string> ml)
		{
			ml.Sort();
			string last = String.Empty;
			for (int n = ml.Count - 1; n >= 0; n--)
			{
				string m = ml[n];
				if (last.IndexOf(m) == 0)
					ml.RemoveAt(n);
				last = m;
			}
		}

		string Header()
		{
			return $"{name} {version}";
		}

		void CheckVersion(List<string> list)
		{
			string line = list[0];
			if (line.Contains(name))
				list.RemoveAt(0);
			else
				DeleteDuplicates(list);
		}

		public bool Load(string p)
		{
			path = p;
			moves.Clear();
			return AddFile(p);
		}

		public bool AddFile(string p)
		{
			bool result = false;
			if (File.Exists(p))
			{
				string ext = Path.GetExtension(p);
				if (ext == ".pgn")
					AddFilePgn(p);
				else
				{
					var list = File.ReadAllLines(p).ToList();
					CheckVersion(list);
					moves.AddRange(list);
				}
				ShowCountLines();
				result = true;
			}
			return result;
		}

		void AddFilePgn(string p)
		{
			List<string> listPgn = File.ReadAllLines(p).ToList();
			foreach (string m in listPgn)
			{
				string cm = m.Trim();
				if (cm.Length < 1)
					continue;
				if (cm[0] == '[')
					continue;
				cm = Regex.Replace(cm, @"\.(?! |$)", ". ");
				string[] arrMoves = cm.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				Chess.SetFen();
				string movesUci = String.Empty;
				foreach (string san in arrMoves)
				{
					if (Char.IsDigit(san[0]))
						continue;
					string umo = Chess.SanToUmo(san);
					if (umo == String.Empty)
						break;
					movesUci += $" {umo}";
					int emo = Chess.UmoToEmo(umo);
					Chess.MakeMove(emo);
				}
				moves.Add(movesUci.Trim());
			}
			DeleteDuplicates(moves);
		}

		public void Save()
		{
			Save(path);
		}

		public void Save(string p)
		{
			path = p;
			string ext = Path.GetExtension(path);
			if (ext == String.Empty)
			{
				ext = defExt;
				path += defExt;
			}
			if (ext == defExt)
				SaveUmo();
			else if (ext == ".pgn")
				SavePgn();
			else
				SaveUci();
		}

		public void SaveUmo(string p)
		{
			path = p;
			SaveUmo();
		}

		void SaveUmo()
		{
			DeleteDuplicates(moves);
			string header = Header();
			moves.Insert(0, header);
			File.WriteAllLines(path, moves);
			moves.RemoveAt(0);
		}

		public void SaveUci(string p)
		{
			path = p;
			SaveUci();
		}

		void SaveUci()
		{
			DeleteDuplicates(moves);
			File.WriteAllLines(path, moves);
		}

		public void SavePgn(string p)
		{
			path = p;
			SavePgn();
		}

		void SavePgn()
		{
			DeleteDuplicates(moves);
			List<string> listPgn = new List<string>();
			foreach (string m in moves)
			{
				string[] arrMoves = m.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				Chess.SetFen();
				string png = String.Empty;
				foreach (string umo in arrMoves)
				{
					string san = Chess.UmoToSan(umo);
					if (san == String.Empty)
						break;
					int number = (Chess.g_moveNumber >> 1) + 1;
					if (Chess.whiteTurn)
						png += $" {number}. {san}";
					else
						png += $" {san}";
					int emo = Chess.UmoToEmo(umo);
					Chess.MakeMove(emo);
				}
				listPgn.Add(String.Empty);
				listPgn.Add("[White \"White\"]");
				listPgn.Add("[Black \"Black\"]");
				listPgn.Add(String.Empty);
				listPgn.Add(png.Trim());
			}
			File.WriteAllLines(path, listPgn);
		}

	}

}
