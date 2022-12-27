using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//klasa koja opisuje jedan predmet (npr. Matematika)
	class Predmet
	{
		//ID predmeta (0 za prazni sat, -1 za sve predmete)
		public int id;
		//naziv predmeta
		public String naziv;
		//skraćeni naziv predmeta
		public String nazivSkraceni;

		//konstruktor za pazan sat
		public Predmet()
		{
			this.id = 0;
			this.naziv = "-";
			this.nazivSkraceni = "-";
		}

		public Predmet(int id, String naziv, String nazivSkraceni)
		{
			this.id = id;
			this.naziv = naziv;
			this.nazivSkraceni = nazivSkraceni;
		}
	}
}
