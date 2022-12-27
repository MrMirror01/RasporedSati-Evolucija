using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//Klasa koja opisuje jedan razred (npr. 3. RT)
	class Razred
	{
		//godina (npr. 3)
		public int godina;
		//naziv smjera (npr. Računalni tehničar)
		public String smjer;
		//skraćeni naziv smjera (npr. RT)
		public String smjerID;

		//lista podatci o predmetima koje ima razred
		public List<PredmetInfo> predmeti = new List<PredmetInfo>();

		public Razred()
		{
			godina = 0;
			smjer = "";
			smjerID = "";
		}

		public Razred(int godina, String smjer, String smjerID, List<PredmetInfo> predmeti)
		{
			this.godina = godina;
			this.smjer = smjer;
			this.smjerID = smjerID;
			this.predmeti = predmeti;
		}
	}
}
