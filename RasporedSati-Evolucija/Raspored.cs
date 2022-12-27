using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//klasa koja predstavlja raspored jednog razreda
	class Raspored
	{
		//razred za koji je raspored
		public Razred razred;
		//raspored u obliku liste(dani) lista(id predmeta po satu)
		public List<List<int>> raspored;

		public const int BROJ_DANA = 6, BROJ_SATI = 9;

		private static Random ran = new Random();

		public Raspored()
		{
			razred = new Razred();
			raspored = new List<List<int>>();
		}

		//za izradu kopije rasporeda
		public Raspored(Raspored raspored)
		{
			this.razred = raspored.razred;
			this.raspored = new List<List<int>>();

			foreach (List<int> dan in raspored.raspored)
			{
				this.raspored.Add(new List<int>());
				foreach (int sat in dan)
				{
					this.raspored[this.raspored.Count - 1].Add(sat);
				}
			}
		}

		public Raspored(Razred razred)
		{
			this.razred = razred;
			raspored = new List<List<int>>();
			//popunimo raspored sa praznim listama (dani)
			for (int i = 0; i < BROJ_DANA; i++) raspored.Add(new List<int>());

			//POPUNIMO RASPORED PREDMETIMA
			int dan = 0; // dan na koji dodajemo predmet
			//doadavamo predmete na raspored
			foreach (PredmetInfo info in razred.predmeti)
			{
				for (int i = 0; i < info.satiTjedno; i++)
				{
					raspored[dan].Add(info.predmet.id);
					dan++; //prelazimo na slijedeci dan
					dan %= BROJ_DANA; //vracamo se na PON ako smo presli SUB
				}
			}
			//popunimo ostatak rasporeda praznim satima
			while (raspored[5].Count < BROJ_SATI)
			{
				raspored[dan].Add(0); //dodajemo prazan sat
				dan++; //prelazimo na slijedeci dan
				dan %= BROJ_DANA; //vracamo se na PON ako smo presli SUB
			}

			//nasumično izmješamo poredak sati u rasporedu
			Shuffle();
		}

		//funkcija koja nasumično izmješa poredak sati u rasporedu
		private void Shuffle()
		{
			//ponovimo onoliko puta koliko ukupno ima sati u tjednu
			for (int i = 0; i < BROJ_DANA * BROJ_SATI; i++)
			{
				//index zadnjeg sata koji gledamo (oni poslije su već izmiješani)
				int last = BROJ_SATI * BROJ_DANA - i - 1;
				//izaberemo nasumični sat od onih koje gledamo [0, last]
				int idx = ran.Next(last + 1);
				//zamijenimo mjesta nasumično izabraom satu i zadnjem gledanom satu
				int temp = raspored[idx / BROJ_SATI][idx % BROJ_SATI];
				raspored[idx / BROJ_SATI][idx % BROJ_SATI] = raspored[last / BROJ_SATI][last % BROJ_SATI];
				raspored[last / BROJ_SATI][last % BROJ_SATI] = temp;
			}
		}

		public override string ToString()
		{
			string output = "";
			for (int sat = 0; sat < raspored[0].Count; sat++)
			{
				for (int dan = 0; dan < raspored.Count; dan++)
				{
					Predmet pred = Info.GetPredmetByID(raspored[dan][sat]);
					if (pred.naziv.Length < 10)
					{
						output += pred.naziv.PadRight(10);
					}
					else
					{
						output += pred.nazivSkraceni.PadRight(10);
					}
				}
				output += "\n";
			}
			return output;
		}
	}
}
