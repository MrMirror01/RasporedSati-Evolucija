using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RasporedSati_Evolucija
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Predmet mat = new Predmet(1, "Matematika", "MAT");
			Predmet hj = new Predmet(2, "Hrvatski", "HJ");
			Predmet sjwp = new Predmet(3, "Skriptni jezici i web programiranje", "SJWP");
			Predmet prMat = new Predmet(4, "Primjena matematike", "PRIMMAT");
			Predmet bp = new Predmet(5, "Dizajn baza podataka", "BP");
			Predmet os = new Predmet(6, "Operacijski sustavi", "OS");
			Predmet oop = new Predmet(7, "NiOP", "OOP");
			Predmet mc = new Predmet(8, "Mikroupravljaci", "MIKRO");
			Predmet gr = new Predmet(9, "Građa računala", "GRADA");
			Predmet eng = new Predmet(10, "Engleski", "ENG");
			Predmet progMob = new Predmet(11, "Programiranje mobitela", "PROGMOB");
			Predmet fiz = new Predmet(12, "Fizika", "FIZ");
			Predmet tzk = new Predmet(13, "Tjelesni", "TZK");
			Predmet rm = new Predmet(14, "Računalne mreže", "RM");

			Info.predmeti.Add(mat);
			Info.predmeti.Add(hj);
			Info.predmeti.Add(sjwp);
			Info.predmeti.Add(prMat);
			Info.predmeti.Add(bp);
			Info.predmeti.Add(os);
			Info.predmeti.Add(oop);
			Info.predmeti.Add(mc);
			Info.predmeti.Add(gr);
			Info.predmeti.Add(eng);
			Info.predmeti.Add(progMob);
			Info.predmeti.Add(fiz);
			Info.predmeti.Add(tzk);
			Info.predmeti.Add(rm);

			Profesor prof1 = new Profesor("123", "Marko", "Čmarko");
			Profesor prof2 = new Profesor("321", "Marac", "Čmarac");

			Razred rt3 = new Razred(3, "Računalni tehničar", "RT", new List<PredmetInfo>()
			{
				new PredmetInfo(mat, prof1, 3),
				new PredmetInfo(hj, prof2, 3),
				new PredmetInfo(sjwp, prof2, 2),
				new PredmetInfo(prMat, prof2, 1),
				new PredmetInfo(bp, prof2, 2),
				new PredmetInfo(os, prof2, 2),
				new PredmetInfo(oop, prof2, 2),
				new PredmetInfo(mc, prof2, 2),
				new PredmetInfo(gr, prof2, 3),
				new PredmetInfo(eng, prof2, 3),
				new PredmetInfo(progMob, prof2, 2),
				new PredmetInfo(fiz, prof2, 2),
				new PredmetInfo(tzk, prof2, 2),
				new PredmetInfo(rm, prof2, 2)
			});
			List<Raspored> generacija = new List<Raspored>();

			for (int i = 0; i < 100; i++)
				generacija.Add(new Raspored(rt3));
			for (int i = 0; i < 1000; i++)
				generacija = Evolucija.NextGen(generacija);
			generacija = generacija.OrderBy(x => -Evolucija.Fitness(x)).ToList();
			debugOutput.Content = generacija[0];
		}

		//doda novo pravilo na listu
		private void addPravioloButton_Click(object sender, RoutedEventArgs e)
		{
			PraviloListItem novoPravilo = new PraviloListItem();

			pravilaList.Items.Add(novoPravilo.grid);
		}

		//Kada promijenimo tip pravila, dinamički generira potrebne UI elemente za to pravilo
		public static void praviloTypeChanged(object sender, EventArgs e)
		{
			//dropdown na kojem se dogodila promijena
			ComboBox select = (ComboBox)sender;
			//pravilo koje gledamo
			Grid grid = (Grid)select.Parent;

			//maknemo sve elemente od trenutnog tipa pravila
			for (int i = 0; i < grid.Children.Count; i++)
			{
				FrameworkElement child = (FrameworkElement)grid.Children[i];
				if (child.Tag.ToString() == "Remove")
				{
					grid.Children.Remove(child);
					i--;
				}
			}

			//dodamo grid u koji ćemo dodat elemente vezane za t vrstu pravila
			Grid specificElements = new Grid();
			specificElements.HorizontalAlignment = HorizontalAlignment.Center;
			specificElements.Tag = "Remove"; //postavimo da se miće tijekom promijene vrste pravila
			
			//ako je trenutno pravilo 'Pozicija'
			if (select.SelectedItem.ToString() == "Pozicija")
			{
				//dodamo tri stupca: za predmet, dan i sat
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());

				//dodamo dropdown za predmet
				ComboBox predmetCombo = new ComboBox();
				Grid.SetColumn(predmetCombo, 0);
				predmetCombo.Tag = "Remove";
				predmetCombo.Items.Add("Bilo koji");
				predmetCombo.Items.Add("Prazan sat");
				foreach (Predmet predmet in Info.predmeti)
				{
					predmetCombo.Items.Add(predmet.nazivSkraceni);
				}
				predmetCombo.SelectedItem = predmetCombo.Items[0];
				specificElements.Children.Add(predmetCombo);

				//dodamo dropdown za dan u tjednu
				ComboBox danCombo = new ComboBox();
				Grid.SetColumn(danCombo, 1);
				danCombo.Tag = "Remove";
				danCombo.Items.Add("Bilo koji");
				danCombo.Items.Add("PON");
				danCombo.Items.Add("UTO");
				danCombo.Items.Add("SRI");
				danCombo.Items.Add("ČET");
				danCombo.Items.Add("PET");
				danCombo.Items.Add("SUB");
				danCombo.SelectedItem = danCombo.Items[0];
				specificElements.Children.Add(danCombo);

				//dodamo dropdown za sat
				ComboBox satCombo = new ComboBox();
				Grid.SetColumn(satCombo, 2);
				satCombo.Tag = "Remove";
				satCombo.Items.Add("Bilo koji");
				for (int i = 0; i < Raspored.BROJ_SATI; i++)
				{
					satCombo.Items.Add((i + 1).ToString() + ".");
				}
				satCombo.SelectedItem = satCombo.Items[0];
				specificElements.Children.Add(satCombo);
			}
			//ako je tip prvila 'Blok'
			else if (select.SelectedItem.ToString() == "Blok")
			{
				//trebamo tri stupca: za predmet, bro sati u bloku i je li strogo definirano
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());
				specificElements.ColumnDefinitions.Add(new ColumnDefinition());

				//dodamo dropdown za izbor predmeta
				ComboBox predmetCombo = new ComboBox();
				Grid.SetColumn(predmetCombo, 0);
				predmetCombo.Tag = "Remove";
				predmetCombo.Items.Add("Bilo koji");
				predmetCombo.Items.Add("Prazan sat");
				foreach (Predmet predmet in Info.predmeti)
				{
					predmetCombo.Items.Add(predmet.nazivSkraceni);
				}
				predmetCombo.SelectedItem = predmetCombo.Items[0];
				specificElements.Children.Add(predmetCombo);

				//dodamo dropdown za broj sati u bloku
				ComboBox brojSatiCombo = new ComboBox();
				Grid.SetColumn(brojSatiCombo, 1);
				brojSatiCombo.Tag = "Remove";
				for (int i = 1; i < Raspored.BROJ_SATI; i++)
				{
					brojSatiCombo.Items.Add((i + 1).ToString());
				}
				brojSatiCombo.SelectedItem = brojSatiCombo.Items[0];
				specificElements.Children.Add(brojSatiCombo);

				//dodamo check box za mora li blok sati biti tocno te duljine ili ne
				CheckBox tocnoCheck = new CheckBox();
				Grid.SetColumn(tocnoCheck, 2);
				tocnoCheck.HorizontalAlignment = HorizontalAlignment.Center;
				tocnoCheck.VerticalAlignment = VerticalAlignment.Center;
				specificElements.Children.Add(tocnoCheck);
			}
			
			//dodamo novonaravljeni grid(specificElements)
			grid.Children.Add(specificElements);
		}
	}

	class PraviloListItem
	{
		//container za ostale dijelove
		public Grid grid;
		public ComboBox pravilaCombo;
		public ComboBox prioritet;

		public PraviloListItem()
		{
			grid = new Grid();

			//inicijaliziramo dropdown za vrstu pravila
			pravilaCombo = new ComboBox();
			pravilaCombo.Items.Add("Pozicija");
			pravilaCombo.Items.Add("Blok");
			pravilaCombo.SelectedItem = pravilaCombo.Items[0];
			pravilaCombo.HorizontalAlignment = HorizontalAlignment.Left;
			pravilaCombo.Tag = "Keep"; //tag ka znamo ka ostaje prilikom promijene vrste pravila
			grid.Children.Add(pravilaCombo); //dodamo na grid
			pravilaCombo.SelectionChanged += MainWindow.praviloTypeChanged; //dodamo event listener

			//inicijaliziramo dropdown za prioritet
			prioritet = new ComboBox();
			prioritet.Items.Add("Zabranjeno");
			for (int i = -10; i <= 10; i++)
			{
				prioritet.Items.Add(i.ToString());
			}
			prioritet.Items.Add("Obavezno");
			prioritet.SelectedItem = prioritet.Items[prioritet.Items.IndexOf("0")];
			prioritet.HorizontalAlignment = HorizontalAlignment.Right;
			prioritet.Tag = "Keep";//ostaje prilikom promijene vrste pravila
			grid.Children.Add(prioritet); //dodamo na grid

			//pozovemo funkciju za dinamičko generiranje parametara za pojedino pravilo
			MainWindow.praviloTypeChanged(pravilaCombo, new EventArgs());
		}
	}
}
