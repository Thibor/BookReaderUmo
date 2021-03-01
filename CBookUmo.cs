using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSChess;

namespace RapBookUci
{
	public class CBookUmo
	{
		string path = "";
		public const string defExt = ".umo";
		string name = "BookReaderUmo";
		readonly string version = "2020-12-01";
		public List<string> moves = new List<string>();
		private static readonly Random random = new Random();
		readonly CChess Chess = new CChess();

		public void Clear()
		{
			moves.Clear();
		}

		public string GetMove(string m)
		{
			if (moves.Count < 1)
				return "";
			int indexL = 0;
			int indexH = moves.Count - 1;
			m = m.Trim();
			if (m.Length > 0)
			{
				while (moves[indexL].IndexOf(m) != 0)
				{
					indexL++;
					if (indexL > indexH)
						return "";
				}
				while (moves[indexH].IndexOf(m) != 0)
				{
					indexH--;
					if (indexL > indexH)
						return "";
				}
			}
			int index = random.Next(indexL, indexH + 1);
			string[] mo = m.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string[] mr = moves[index].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (mr.Length > mo.Length)
				return mr[mo.Length];
			return "";
		}

		void DeleteDuplicates(List<string> ml)
		{
			ml.Sort();
			string last = "";
			for (int n = ml.Count - 1; n >= 0; n--)
			{
				string m = ml[n];
				if(last.IndexOf(m) == 0)
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

		}

		public bool Load(string p, bool clear = true)
		{
			bool result = false;
			path = p;
			if (clear)
				moves.Clear();
			if (File.Exists(path))
			{
				string ext = Path.GetExtension(path);
				if (ext == ".pgn")
					LoadPgn();
				else
				{
					var list = File.ReadAllLines(path).ToList();
					CheckVersion(list);
					moves.AddRange(list);
				}
				result = true;
			}
			return result;
		}

		void LoadPgn()
		{
			List<string> listPgn = File.ReadAllLines(path).ToList();
			foreach (string m in listPgn)
			{
				string cm = m.Trim();
				if (cm.Length < 1)
					continue;
				if (cm[0] == '[')
					continue;
				string[] arrMoves = cm.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				Chess.SetFen();
				string movesUci = "";
				foreach (string san in arrMoves)
				{
					if (Char.IsDigit(san[0]))
						continue;
					string umo = Chess.SanToUmo(san);
					if (umo == "")
						break;
					movesUci += $" {umo}";
					int emo = Chess.UmoToEmo(umo);
					Chess.MakeMove(emo);
				}
				moves.Add(movesUci.Trim());
			}
		}

		public void Save(string p)
		{
			path = p;
			string ext = Path.GetExtension(path);
			if(ext == "")
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
				string[] arrMoves = m.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				Chess.SetFen();
				string png = "";
				foreach (string umo in arrMoves)
				{
					string san = Chess.UmoToSan(umo);
					if (san == "")
						break;
					int number = (Chess.g_moveNumber >> 1) + 1;
					if (Chess.whiteTurn)
						png += $" {number}. {san}";
					else
						png += $" {san}";
					int emo = Chess.UmoToEmo(umo);
					Chess.MakeMove(emo);
				}
				listPgn.Add("");
				listPgn.Add("[White \"White\"]");
				listPgn.Add("[Black \"Black\"]");
				listPgn.Add("");
				listPgn.Add(png.Trim());
			}
			File.WriteAllLines(path, listPgn);
		}

	}

}
