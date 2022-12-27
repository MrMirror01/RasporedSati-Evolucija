using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	abstract class Pravilo
	{
		//prioritet pravila: [-10, 10], -100 za zabranjeno stanje, +100 za obavezno stanje
		public int prioritet;
		//metoda koja vraća rating za pojedini raspored koristeći pravilo
		public abstract int Check(Raspored raspored);
	}

	//za postavljanje prioriteta za pojavljivanje određenog predmeta određeni dan i sat
	class Pozicija : Pravilo
	{
		//predmet koji provjeravamo
		public int predmetID;
		//dan u kojem gledamo, -1 za svaki dan
		public int dan;
		//sat u kojem gledamo, -1 za svaki sat
		public int sat;

		public Pozicija(int predmet, int dan, int sat, int prioritet)
		{
			if (dan == -1 && sat == -1)
			{
				throw new Exception("Krivi argumenti pravila, ne mogu i dan i sat biti neodređeni!");
			}

			this.predmetID = predmet;
			this.dan = dan;
			this.sat = sat;
			this.prioritet = prioritet;
		}

		public Pozicija(Predmet predmet, int dan, int sat, int prioritet)
		{
			if (dan == -1 && sat == -1)
			{
				throw new Exception("Krivi argumenti pravila, ne mogu i dan i sat biti neodređeni!");
			}

			this.predmetID = predmet.id;
			this.dan = dan;
			this.sat = sat;
			this.prioritet = prioritet;
		}

		public override int Check(Raspored raspored)
		{
			//vraćeni rating rasporeda
			int rating = 0;

			//ako su zadani i sat i dan
			if (dan != -1 && sat != -1)
			{
				if ((predmetID == -1 && raspored.raspored[dan][sat] != 0) || raspored.raspored[dan][sat] == predmetID)
					rating += prioritet;
				return rating;
			}

			//ako je zadani dan
			if (dan != -1 && sat == -1)
			{
				for (int predmet = 0; predmet < raspored.raspored[dan].Count; predmet++)
				{
					if ((predmetID == -1 && raspored.raspored[dan][predmet] != 0) || raspored.raspored[dan][predmet] == predmetID)
					{
						rating += prioritet;
					}
				}

				return rating;
			}

			//ako je zadani sat
			for (int day = 0; day < raspored.raspored.Count; day++)
			{
				if ((predmetID == -1 && raspored.raspored[day][sat] != 0) || raspored.raspored[day][sat] == predmetID)
				{
					rating += prioritet;
				}
			}

			return rating;
		}
	}

	//Pravilo za prvjeravanje blokova sati
	class Blok : Pravilo
	{
		//predmet koji promatramo
		public int predmetID;
		//broj sati u bloku
		public int brojSati;
		//mora li biti točno toliko sati, ili može biti i više
		public bool tocno;

		public Blok(int predmet, int velicinaBloka, int prioritet, bool tocno = false)
		{
			predmetID = predmet;
			brojSati = velicinaBloka;
			this.tocno = tocno;
			this.prioritet = prioritet;
		}

		public override int Check(Raspored raspored)
		{
			// rating za taj raspored
			int rating = 0;

			//za svaki dan
			foreach (List<int> dan in raspored.raspored)
			{
				//označava koji je sat bio prije trenutnog
				int last = -1;
				//označava koliko je bilo istih sati za redom
				int zaRedom = 1;
				
				//prolazimo po satima
				foreach (int sat in dan)
				{
					//ako je zadnji sat isti kao trenutni (blok sat)
					if (last == sat)
					{
						//brojimo koliko ih je bilo za redom
						zaRedom++;
						//ako se radi o traženom satu
						if (sat == predmetID || (predmetID == -1 && sat != 0))
						{
							//ako smo došli do željenog broja sati
							if (zaRedom == brojSati)
							{
								//blok sat željene veličine, dodajemo prioritet na rating
								rating += prioritet;
							}
							//ako smo premašili veličinu blok sata, a traži se točnost
							if (tocno && zaRedom == brojSati + 1)
							{
								//maknemo krivo dodani prioritet sa ratinga
								rating -= prioritet;
							}
						}
					}
					else zaRedom = 1; //resetiramo vrjednost

					last = sat; //trenutni sat postaje prijašnji sat
				}
			}

			//vratimo rating rasporeda
			return rating;
		}
	}
}
