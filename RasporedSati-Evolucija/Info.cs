using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//sadrži osnovne informacije
	static class Info
	{
		//lista svih predmeta
		public static List<Predmet> predmeti = new List<Predmet>();

		// metoda za dobivanje predmeta prema ID-u
		public static Predmet GetPredmetByID(int id)
		{
			if (id == 0) return new Predmet();
			//--------------TODO: BINARY SEARCH--------------
			foreach (Predmet predmet in predmeti)
			{
				if (predmet.id == id) return predmet;
			}

			throw new Exception("No predmet of ID " + id.ToString());
		}
	}
}
