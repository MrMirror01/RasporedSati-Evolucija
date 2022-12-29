using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	static class Evolucija
	{
		//postotak jeinki koji prelazi u slijedcu generaciju
		public static double selectionPercent = 0.5;
		//broj top najboljih jedinki koje prelaze u slijedecu generaciju
		public static int elitnihJedinki = 10;
		//maksimalni broj različitih mutacija
		public static int maxMutacija = 3;

		private static Random ran = new Random();

		//funkcija koja za pojedinu generaciu generira sljedecu generaciju
		public static List<Raspored> NextGen(List<Raspored> generacija)
		{
			//slijedeca generacija
			List<Raspored> sljedeca = Selekcija(generacija);
			//index zadnje jedinke koja je presla u novu generaciju
			int maxSelected = sljedeca.Count;
			//dok nova generacia ne dostigne velicinu prethodne
			while (sljedeca.Count < generacija.Count)
			{
				//na novu generaciju dodamo mutiranu neku jedinku koja je potomak dviju jedink iz prosle generacije
				Raspored nova = Krizanje(sljedeca[ran.Next(maxSelected)], sljedeca[ran.Next(maxSelected)]);
				Raspored mut = Mutacija(nova);
				sljedeca.Add(mut);
			}

			return sljedeca;
		}

		//funkcija koja određuje koliko je dobar pojedini raspored
		public static double Fitness(Raspored raspored)
		{
			int fitness = 0;			
			
			foreach (Pravilo pravilo in Info.pravila)
			{
				fitness += pravilo.Check(raspored);
			}
			return fitness;
		}

		//funkcija koja vraća listu rasporeda koji su prošli selekciju
		//Roulette wheel selection
		public static List<Raspored> Selekcija(List<Raspored> rasporedi)
		{
			//output funkcije
			List<Raspored> output = new List<Raspored>();

			//fitness vrijednosti za pojedini raspored
			Dictionary<Raspored, double> fitness = new Dictionary<Raspored, double>();

			//izračunamo fitness vrijednost svih rasporeda
			foreach (Raspored raspored in rasporedi)
			{
				fitness[raspored] = Fitness(raspored);
			}

			//IMPLEMENTACIJA ELITNIH JEDINKI
			//sortiramo edinke prema fitnessu (-fitness jer je ascending order)
			rasporedi = rasporedi.OrderBy(x => -fitness[x]).ToList();
			//najboljih 'elitnihJedinki' jedinki dodamo na output
			for (int i = 0; i < elitnihJedinki; i++)
			{
				output.Add(rasporedi[i]);
			}

			//najmanji fitness value
			double minFitness = fitness[rasporedi[rasporedi.Count - 1]];
			//zbroj fitness vrijednosti svih 
			double sumOfFitnessValues = 0;
			for (int i = 0; i < rasporedi.Count; i++)
			{
				//od svih fitness vrijednosti prvo oduzmemo najmanju kako bi se rijesili negativnih vrjednosti
				fitness[rasporedi[i]] -= minFitness;
				//izracunamo zbroj svih vrijednosti
				sumOfFitnessValues += fitness[rasporedi[i]];
			}

			//normaliziramo fitness vrijednosti (podijelimo sa zbrojom svih vrijednosti)
			//zatim ih pripremim za nasumično biranje (izračunamo 'prefix sumu' kako bi svaka jednika
			//imala svoj interval untar intervala [0, 1])
			for (int i = 0; i < rasporedi.Count; i++)
			{
				if (i != 0) 
					fitness[rasporedi[i]] = fitness[rasporedi[i - 1]] + (fitness[rasporedi[i]] / sumOfFitnessValues);
				else
					fitness[rasporedi[i]] /= sumOfFitnessValues;
			}

			//selectionPercnt * ukupnoRasporedi puta generiramo nasumičan broj i biramo jedinku koja prelazi
			//u slijedecu generacju
			for (int i = 0; i < rasporedi.Count * selectionPercent - elitnihJedinki; i++)
			{
				//nasumicni broj koji određuje koja jedinka prolazi u slijedecu generaciju
				double rnd = ran.NextDouble();
				//BINARY SEARCH koji trazi tu jedinku
				//lo i hi su indexi rasporeda u polju rasporedi
				int lo = 0, hi = rasporedi.Count - 1;
				while (lo < hi)
				{
					int mid = (lo + hi) / 2;
					if (rnd < fitness[rasporedi[mid]])
					{
						hi = mid;
					}
					else
					{
						lo = mid + 1;
					}
				}
				//na output dodajemo jedinku koju smo nasli
				//(koja je na indexu lo jer je lo = hi = mid)
				output.Add(rasporedi[lo]);
			}

			return output;
		}

		//funkcija krizanja za dva rasporeda, vraća raspored koji je neka kombinacija dva ulazna rasporeda
		public static Raspored Krizanje(Raspored prvi, Raspored drugi)
		{
			if (prvi.razred != drugi.razred)
			{
				throw new Exception("Poksaj krizanja dva rasporeda drugacijih razreda");
			}

			//u 50% slucajeva zamijenimo prvi i drugi rasored
			if (ran.NextDouble() < .5)
			{
				Raspored temp = prvi;
				prvi = drugi;
				drugi = temp;
			}

			Raspored child = new Raspored();
			child.razred = prvi.razred;

			//napravimo kopiju liste predmeta
			List<PredmetInfo> predmeti = new List<PredmetInfo>();
			foreach (PredmetInfo pred in prvi.razred.predmeti)
			{
				predmeti.Add(pred);
			}

			//za svaki predmet koliko smo ih dodali u novi raspored
			int[] potrosenoPredmeta = new int[predmeti.Count + 1];
			//kopija rasporeda koja sprema koje smo predmete već dodali na djete
			List<List<bool>> addedPrvi = new List<List<bool>>();
			List<List<bool>> addedDrugi = new List<List<bool>>();

			//generiramo novi raspored
			//sate koji se podudaraju u oba rasporeda automatski dodamo u novi raspored
			for (int i = 0; i < Raspored.BROJ_DANA; i++)
			{
				child.raspored.Add(new List<int>());
				addedPrvi.Add(new List<bool>());
				addedDrugi.Add(new List<bool>());
				for (int j = 0; j < Raspored.BROJ_SATI; j++)
				{
					//ako se sat podudaraju
					if (prvi.raspored[i][j] == drugi.raspored[i][j])
					{
						//dodamo taj sat na nov raspored
						child.raspored[i].Add(prvi.raspored[i][j]);
						//zapamtimo da smo ga iskoristil
						potrosenoPredmeta[prvi.raspored[i][j]]++;
						//oznacimo da je taj predmet vec bio dodan
						addedPrvi[i].Add(true);
						addedDrugi[i].Add(true);
					}
					else
					{
						//dodamo prazan sat na novi raspored (za sad)
						child.raspored[i].Add(0);
						//oznacimo da taj predmet jos nije bio dodan
						addedPrvi[i].Add(false);
						addedDrugi[i].Add(false);
					}
				}
			}

			//izmjesamo poredak predmeta
			for (int i = 0; i < predmeti.Count; i++)
			{
				//index zadnjeg predmeta koji gledamo (oni poslije su već izmiješani)
				int last = predmeti.Count - i - 1;
				//izaberemo nasumični predmet od onih koje gledamo [0, last]
				int idx = ran.Next(last + 1);
				//zamijenimo mjesta nasumično izabraom predmetu i zadnjem gledanom predmetu
				PredmetInfo temp = predmeti[idx];
				predmeti[idx] = predmeti[last];
				predmeti[last] = temp;
			}

			//bool koji odreduje koji je raspored trenutno 'glavni'
			//false => prvi je glavni, true => drugi je glavni
			bool glavni = false;
			for (int i = 0; i < predmeti.Count; i++)
			{
				int trenPredmet = predmeti[i].predmet.id;
				while (potrosenoPredmeta[predmeti[i].predmet.id] < predmeti[i].satiTjedno)
				{
					DodajPredmet(trenPredmet, child, (!glavni ? prvi : drugi), (!glavni ? addedPrvi : addedDrugi));
					potrosenoPredmeta[predmeti[i].predmet.id]++;
				}
				glavni = !glavni;
			}

			return child;
		}

		//funkija koja doda predmet na najoptimalniju poziciju u rasporedu
		private static void DodajPredmet(int id, Raspored child, Raspored parrent, List<List<bool> > added)
		{
			//prolazimo po rasporedu
			for (int dan = 0; dan < Raspored.BROJ_DANA; dan++)
			{
				for (int sat = 0; sat < Raspored.BROJ_SATI; sat++)
				{
					//ako nađemo pojavljivanje tog predmeta koje jos nije dodano
					if (parrent.raspored[dan][sat] == id && !added[dan][sat])
					{
						//dodamo taj predmet na najblize slobodno mjesto
						added[dan][sat] = true;
						NadiNajblizi(id, child, dan, sat);
						return;
					}
				}
			}
		}

		public static void NadiNajblizi(int id, Raspored child, int dan, int sat)
		{
			//delta dan (promijena po danu)
			for (int dDan = 0; dan - dDan >= 0 || dan + dDan < Raspored.BROJ_DANA; dDan++)
			{
				//delta sat (promijena po danu)
				for (int dSat = 0; sat - dSat >= 0 || sat + dSat < Raspored.BROJ_SATI; dSat++)
				{
					//neg -> dan/sat koji je delta dana/sata prije
					//pos -> dan/sat koii je delta dana/sata poslije
					int negDan = dan - dDan;
					int posDan = dan + dDan;
					int negSat = sat - dSat;
					int posSat = sat + dSat;

					//ako nađemo slobodan sat unutar granica rasporeda -> dodamo predmet
					if (negDan >= 0)
					{
						if (negSat >= 0 && child.raspored[negDan][negSat] == 0)
						{
							child.raspored[negDan][negSat] = id;
							return;
						}
						if (posSat < Raspored.BROJ_SATI && child.raspored[negDan][posSat] == 0)
						{
							child.raspored[negDan][posSat] = id;
							return;
						}
					}
					if (posDan < Raspored.BROJ_DANA)
					{
						if (negSat >= 0 && child.raspored[posDan][negSat] == 0)
						{
							child.raspored[posDan][negSat] = id;
							return;
						}
						if (posSat < Raspored.BROJ_SATI && child.raspored[posDan][posSat] == 0)
						{
							child.raspored[posDan][posSat] = id;
							return;
						}
					}
				}
			}
		}

		//funkcija mutacije, zamijeni mjesta nekim predmetima
		public static Raspored Mutacija(Raspored raspored)
		{
			//novi je deepcopy od raspored
			Raspored noviRaspored = new Raspored(raspored);

			int brMutacija = ran.Next(maxMutacija + 1);
			for (int i = 0; i < brMutacija; i++)
			{
				//generiramo dva nasumicne pozicije u rasporedu
				int dan1 = ran.Next(Raspored.BROJ_DANA);
				int sat1 = ran.Next(Raspored.BROJ_SATI);
				int dan2 = ran.Next(Raspored.BROJ_DANA);
				int sat2 = ran.Next(Raspored.BROJ_SATI);

				//zamijenimo mjesta predmetima na generiranim pozicijama
				int temp = noviRaspored.raspored[dan1][sat1];
				noviRaspored.raspored[dan1][sat1] = raspored.raspored[dan2][sat2];
				noviRaspored.raspored[dan2][sat2] = temp;
			}
			return noviRaspored;
		}
	}
}
