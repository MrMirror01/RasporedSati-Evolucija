using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//klasa koja sadrži podatke o nekom predmetu za pojedini razred
	class PredmetInfo
	{
		//predmet o kojem su podatci
		public Predmet predmet;
		//profesor koji ga predaje
		public Profesor profesor;
		//broj sati tjedno
		public int satiTjedno;

		public PredmetInfo(Predmet predmet, Profesor profesor, int satiTjedno)
		{
			this.predmet = predmet;
			this.profesor = profesor;
			this.satiTjedno = satiTjedno;
		}
	}
}
