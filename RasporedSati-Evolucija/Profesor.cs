using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasporedSati_Evolucija
{
	//klasa koja opisuje jednog profesora
	class Profesor
	{
		//OIB profesora
		public String oib;
		//ime profesora
		public String ime;
		//prezime profesora
		public String prezime;

		public Profesor(String oib, String ime, String prezime)
		{
			this.oib = oib;
			this.ime = ime;
			this.prezime = prezime;
		}
	}
}
