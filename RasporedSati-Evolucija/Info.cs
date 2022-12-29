using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RasporedSati_Evolucija
{
	//sadrži osnovne informacije
	static class Info
	{
		//lista svih predmeta
		public static List<Predmet> predmeti = new List<Predmet>();
		//lista pravila evolucije
		public static List<Pravilo> pravila = new List<Pravilo>(){
				new Pozicija(0, -1, 0, -2),
				new Pozicija(0, -1, 1, -3),
				new Pozicija(0, -1, 2, -3),
				new Pozicija(0, -1, 3, -3),
				new Pozicija(0, -1, 4, -3),
				new Pozicija(0, -1, 5, -3),
				new Pozicija(0, -1, 6, -1),
				new Pozicija(-1, 5, -1, -3),
				new Pozicija(1, -1, 0, 2),
				new Pozicija(1, -1, 1, 1),
				new Blok(-1, 2, 2, true)
		};

		// metoda za dobivanje predmeta prema ID-u
		public static Predmet GetPredmetByID(int id)
		{
			if (id == 0) return new Predmet();
			foreach (Predmet predmet in predmeti)
			{
				if (predmet.id == id) return predmet;
			}

			throw new Exception("No predmet of ID " + id.ToString());
		}

		// metoda za učitavanje pravila iz UI liste pravila
		public static void LoadPrvilaFromGUI(ItemCollection listaPravila)
		{
			pravila.Clear();

			foreach (Grid grid in listaPravila)
			{
				ComboBox vrstaPravilaCombo = new ComboBox();
				Grid parametres = new Grid();
				ComboBox prioritet = new ComboBox();
				int prioritetNum = 0;

				foreach (FrameworkElement element in grid.Children)
				{
					switch (element.Tag.ToString())
					{
						case "pravilo":
							vrstaPravilaCombo = (ComboBox)element;
							break;
						case "remove":
							parametres = (Grid)element;
							break;
						case "prioritet":
							prioritet = (ComboBox)element;
							break;
					}
				}

				switch (prioritet.SelectedIndex)
				{
					case 0:
						prioritetNum = -100;
						break;
					case 22:
						prioritetNum = 100;
						break;
					default:
						prioritetNum = prioritet.SelectedIndex - 11;
						break;
				}
				
				switch (vrstaPravilaCombo.SelectedItem.ToString())
				{
					case "Pozicija":
						int predmetID = -1, dan = -1, sat = -1;
						foreach (FrameworkElement element in parametres.Children)
						{
							switch (element.Tag.ToString())
							{
								case "predmet":
									predmetID = ((ComboBox)element).SelectedIndex - 1;
									break;
								case "dan":
									dan = ((ComboBox)element).SelectedIndex - 1;
									break;
								case "sat":
									sat = ((ComboBox)element).SelectedIndex - 1;
									break;
							}
						}
						pravila.Add(new Pozicija(predmetID, dan, sat, prioritet.SelectedIndex));
						break;
				}
			}
		}
	}
}
