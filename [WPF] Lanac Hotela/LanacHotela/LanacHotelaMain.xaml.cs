using LanacHotelaServiceLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LanacHotela
{
    public partial class LanacHotelaMain : Window
    {
        private readonly List<Hotel> selectedHotels = new List<Hotel>();

        private readonly List<AranzmanDetaljno> aranzmani = new List<AranzmanDetaljno>();
        private readonly List<HotelFinansije> finansije = new List<HotelFinansije>();
        private readonly List<Kontakt> kontakti = new List<Kontakt>();
        private readonly List<Hotel> hoteli = new List<Hotel>();
        private readonly List<Soba> sobe = new List<Soba>();
        private readonly List<Gost> gosti = new List<Gost>();

        public LanacHotelaMain()
        {
            InitializeComponent();
            InitializeApp();
            PopulateHotelSelector();
            UpdateDataGrid();
        }

        private void InitializeApp()
        {
            aranzmaniDataGrid.ItemsSource = aranzmani;
            hotelDataGrid.ItemsSource = hoteli;
            kontaktiDataGrid.ItemsSource = kontakti;
            finansijeDataGrid.ItemsSource = finansije;
            sobeDataGrid.ItemsSource = sobe;
            gostiDataGrid.ItemsSource = gosti;

            hotelsComboBox.SelectionChanged += HotelSelectionChanged;

            noviKontaktTip.Items.Add("email");
            noviKontaktTip.Items.Add("telefon");

            noviHotelZvjezdice.Items.Add(1);
            noviHotelZvjezdice.Items.Add(2);
            noviHotelZvjezdice.Items.Add(3);
            noviHotelZvjezdice.Items.Add(4);
            noviHotelZvjezdice.Items.Add(5);
        }

        private async void HotelChecked(object sender, RoutedEventArgs e)
        {
            dynamic checkboxData = e.OriginalSource;
            bool isChecked = checkboxData.IsChecked;
            int hotelID = int.Parse(checkboxData.CommandParameter);

            HotelService hotelService = new HotelService();
            Hotel checkedHotel = await hotelService.GetById(hotelID);

            if (isChecked)
            {
                selectedHotels.Add(checkedHotel);
            }
            else
            {
                selectedHotels.Remove(checkedHotel);
            }

            UpdateDataGrid();
        }

        private async void PopulateHotelSelector()
        {
            stackPanel.Children.Clear();
            HotelService hotelService = new HotelService();

            foreach (Hotel h in await hotelService.GetAll())
            {
                CheckBox checkBox = new CheckBox() { Content = h.Ime, CommandParameter = h.HotelID.ToString() };
                checkBox.Click += HotelChecked;
                stackPanel.Children.Add(checkBox);
            }
        }

        private async void UpdateDataGrid()
        {
            // Aranzmani i finansije
            aranzmani.Clear();
            finansije.Clear();

            AranzmanDetaljnoService aranzmanService = new AranzmanDetaljnoService();
            HotelFinansijeService finansijeService = new HotelFinansijeService();

            foreach (Hotel h in selectedHotels)
            {
                foreach (AranzmanDetaljno aranzman in await aranzmanService.GetAllForHotel(h.HotelID))
                {
                    if (aranzman != null)
                    {
                        aranzmani.Add(aranzman);
                    }
                }

                HotelFinansije finansija = await finansijeService.GetById(h.HotelID);
                if (finansija != null)
                {
                    finansije.Add(finansija);
                }
            }
            aranzmaniDataGrid.Items.Refresh();
            finansijeDataGrid.Items.Refresh();

            // Hoteli
            hoteli.Clear();
            hotelsComboBox.Items.Clear();
            novaSobaHotel.Items.Clear();
            HotelService hotelService = new HotelService();

            foreach (Hotel h in await hotelService.GetAll())
            {
                if (h != null)
                {
                    hoteli.Add(h);
                }

                ComboBoxItem cbHotelItem = new ComboBoxItem
                {
                    Tag = h.HotelID,
                    Content = h.Ime
                };
                hotelsComboBox.Items.Add(cbHotelItem);

                ComboBoxItem cbZaSobeItem = new ComboBoxItem
                {
                    Tag = h.HotelID,
                    Content = h.Ime
                };
                novaSobaHotel.Items.Add(cbZaSobeItem);
            }
            hotelDataGrid.Items.Refresh();
            hotelsComboBox.Items.Refresh();
            novaSobaHotel.Items.Refresh();

            // Sobe
            sobe.Clear();

            SobaService sobaService = new SobaService();
            {
                foreach (Soba soba in await sobaService.GetAll())
                {
                    if (soba != null)
                    {
                        sobe.Add(soba);
                    }
                }
            }
            sobeDataGrid.Items.Refresh();

            // Kontakti
            kontakti.Clear();
            noviHotelKontakt.Items.Clear();
            KontaktService kontaktService = new KontaktService();

            foreach (Kontakt k in await kontaktService.GetAll())
            {
                if (k != null)
                {
                    kontakti.Add(k);
                }

                if (k.HotelID == 0 && k.GostID == 0)
                {
                    noviHotelKontakt.Items.Add(k.KontaktID + ": " + k.Info);
                }
            }
            kontaktiDataGrid.Items.Refresh();

            // Gosti
            gosti.Clear();
            guestComboBox.Items.Clear();
            GostService gostService = new GostService();

            foreach (Gost g in await gostService.GetAll())
            {
                gosti.Add(g);
                ComboBoxItem cbGostItem = new ComboBoxItem
                {
                    Tag = g.GostID,
                    Content = g.Ime + " " + g.Prezime
                };
                guestComboBox.Items.Add(cbGostItem);
            }
            gostiDataGrid.Items.Refresh();
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            AranzmanDetaljno selectedAranzman = (AranzmanDetaljno)aranzmaniDataGrid.SelectedItem;
            AranzmanService aranzmanService = new AranzmanService();

            resultTextBlock.Text = "";

            if (selectedAranzman == null)
            {
                resultTextBlock.Text = "Odaberite aranzman.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            if (await aranzmanService.Delete(selectedAranzman.AranzmanID) == -1)
            {
                resultTextBlock.Text = "Neuspjesno brisanje.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                resultTextBlock.Text = "Aranzman obrisan.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }

            UpdateDataGrid();
        }

        private async void HotelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SobaService sobaService = new SobaService();
            ComboBoxItem item = (ComboBoxItem)hotelsComboBox.SelectedItem;
            if (item == null)
            {
                return;
            }

            int hotelID = (int)item.Tag;

            roomComboBox.Items.Clear();

            foreach (Soba s in await sobaService.GetAll())
            {
                if (s.HotelID == hotelID)
                {
                    ComboBoxItem cbSobaItem = new ComboBoxItem
                    {
                        Tag = s.SobaID,
                        Content = $"Soba {s.SobaID}"
                    };
                    roomComboBox.Items.Add(cbSobaItem);
                }
            }
        }

        private async void AddClick(object sender, RoutedEventArgs e)
        {
            if (noviAranzmanPocetak.SelectedDate == null
                || noviAranzmanKraj.SelectedDate == null
                || hotelsComboBox.SelectedItem == null
                || guestComboBox.SelectedItem == null
                || roomComboBox.SelectedItem == null)
            {
                resultTextBlock.Text = "Popunite sva polja.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            resultTextBlock.Text = "";

            ComboBoxItem hotelItem = (ComboBoxItem)hotelsComboBox.SelectedItem;
            int hotelID = (int)hotelItem.Tag;

            ComboBoxItem gostItem = (ComboBoxItem)guestComboBox.SelectedItem;
            int gostID = (int)gostItem.Tag;

            ComboBoxItem sobaItem = (ComboBoxItem)roomComboBox.SelectedItem;
            int sobaID = (int)sobaItem.Tag;

            Aranzman aranzman = new Aranzman(0, noviAranzmanPocetak.SelectedDate.Value, noviAranzmanKraj.SelectedDate.Value, noviAranzmanJeOtkazan.IsChecked.Value, noviAranzmanJeZavrsen.IsChecked.Value, hotelID, gostID, sobaID);
            AranzmanService aranzmanService = new AranzmanService();

            if (await aranzmanService.Insert(aranzman) == -1)
            {
                resultTextBlock.Text = "Neuspjesno kreiranje.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                resultTextBlock.Text = "Aranzman kreiran.";
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }

            UpdateDataGrid();
        }

        private async void UpdateClick(object sender, RoutedEventArgs e)
        {
            AranzmanService aranzmanService = new AranzmanService();
            resultTextBlock.Text = "";
            int counter = 0;

            foreach (AranzmanDetaljno a in aranzmani)
            {
                if (await aranzmanService.Update(a) != -1)
                {
                    counter++;
                }
            }

            resultTextBlock.Text = "Azurirani unosi.";
            resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void AddClickContact(object sender, RoutedEventArgs e)
        {
            if (noviKontaktTip.SelectedItem == null
                || noviKontaktInfo.Text.Length <= 5)
            {
                rezultatTextKontakt.Text = "Popunite sva polja.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatTextKontakt.Text = "";

            string kontaktTip = (string)noviKontaktTip.SelectedItem;
            string kontaktInfo = noviKontaktInfo.Text;

            Kontakt kontakt = new Kontakt(0, kontaktTip, kontaktInfo, 0, 0);
            KontaktService kontaktService = new KontaktService();

            if (await kontaktService.Insert(kontakt) == -1)
            {
                rezultatTextKontakt.Text = "Neuspjesno kreiranje.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatTextKontakt.Text = "Kontakt kreiran.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);
                UpdateDataGrid();
            }
        }

        private async void UpdateClickContact(object sender, RoutedEventArgs e)
        {
            KontaktService kontaktService = new KontaktService();
            rezultatTextKontakt.Text = "";
            int counter = 0;

            foreach (Kontakt k in kontakti)
            {
                if (k.GostID != 0 && k.HotelID != 0)
                    continue;
                
                if (await kontaktService.Update(k) != -1)
                    counter++;
            }

            rezultatTextKontakt.Text = "Azurirani unosi.";
            rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickContact(object sender, RoutedEventArgs e)
        {
            Kontakt selectedKontakt = (Kontakt)kontaktiDataGrid.SelectedItem;
            KontaktService kontaktService = new KontaktService();

            rezultatTextKontakt.Text = "";

            if (selectedKontakt == null)
            {
                rezultatTextKontakt.Text = "Odaberite aranzman.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await kontaktService.GetAllForHotel(selectedKontakt.HotelID)).Count == 1 || (await kontaktService.GetAllForGuest(selectedKontakt.GostID)).Count == 1)
            {
                rezultatTextKontakt.Text = "Jedini kontakt.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await kontaktService.Delete(selectedKontakt.KontaktID)) == -1)
            {
                rezultatTextKontakt.Text = "Neuspjesno brisanje.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatTextKontakt.Text = "Kontakt obrisan.";
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);
                UpdateDataGrid();
            }
        }

        private async void AddClickHotel(object sender, RoutedEventArgs e)
        {
            if (noviHotelIme.Text.Length <= 1
                || noviHotelZvjezdice.SelectedItem == null
                || noviHotelKontakt.SelectedItem == null
                || noviHotelUlica.Text.Length <= 1
                || noviHotelBroj.Text.Length <= 1
                || noviHotelGrad.Text.Length <= 1
                || noviHotelZIP.Text.Length <= 1
                || noviHotelDrzava.Text.Length <= 1)
            {
                rezultatHotel.Text = "Popunite sva polja.";
                rezultatHotel.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatHotel.Text = "";

            try
            {
                string hotelIme = noviHotelIme.Text;
                int hotelZvjezdice = (int)noviHotelZvjezdice.SelectedItem;
                KontaktService ks = new KontaktService();
                Kontakt hotelKontakt = await ks.GetById(int.Parse(((string)noviHotelKontakt.SelectedItem).Split(':')[0]));
                string hotelUlica = noviHotelUlica.Text;
                string hotelBroj = noviHotelBroj.Text;
                string hotelGrad = noviHotelGrad.Text;
                int hotelZIP = int.Parse(noviHotelZIP.Text);
                string hotelDrzava = noviHotelDrzava.Text;

                Hotel hotel = new Hotel(0, hotelIme, hotelZvjezdice, hotelUlica, hotelBroj, hotelGrad, hotelZIP, hotelDrzava);
                HotelService hotelService = new HotelService();

                if (await hotelService.Insert(hotel) == -1)
                {
                    rezultatHotel.Text = "Neuspjesno kreiranje.";
                    rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    foreach (Hotel h in await hotelService.GetAll())
                    {
                        if (h.Equals(hotel))
                        {
                            hotel.HotelID = h.HotelID;
                        }
                    }

                    KontaktService kontaktService = new KontaktService();
                    await kontaktService.Update(hotelKontakt, hotel.HotelID);

                    rezultatHotel.Text = "Hotel kreiran.";
                    rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);

                    CheckBox checkBox = new CheckBox() { Content = hotel.Ime, CommandParameter = hotel.HotelID.ToString() };
                    checkBox.Click += HotelChecked;
                    stackPanel.Children.Add(checkBox);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickHotel(object sender, RoutedEventArgs e)
        {
            HotelService hotelService = new HotelService();
            rezultatHotel.Text = "";
            int counter = 0;

            foreach (Hotel h in hoteli)
            {
                if (await hotelService.Update(h) != -1)
                {
                    counter++;
                }
            }

            rezultatHotel.Text = "Azurirani unosi.";
            rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);

            if (counter > 0)
            {
                selectedHotels.Clear();
                PopulateHotelSelector();
            }
            UpdateDataGrid();
        }

        private async void DeleteClickHotel(object sender, RoutedEventArgs e)
        {
            Hotel selectedHotel = (Hotel)hotelDataGrid.SelectedItem;
            HotelService hotelService = new HotelService();
            AranzmanService aranzmanService = new AranzmanService();

            rezultatHotel.Text = "";

            if (selectedHotel == null)
            {
                rezultatHotel.Text = "Odaberite hotel.";
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await aranzmanService.GetAllForHotel(selectedHotel.HotelID)).Count > 0)
            {
                rezultatHotel.Text = "Hotel ima aranzmane.";
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await hotelService.Delete(selectedHotel.HotelID)) == -1)
            {
                rezultatHotel.Text = "Neuspjesno brisanje.";
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatHotel.Text = "Hotel obrisan.";
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);
                selectedHotels.Clear();
                PopulateHotelSelector();
                UpdateDataGrid();
            }
        }

        private async void AddClickSobe(object sender, RoutedEventArgs e)
        {
            if (novaSobaBrojSprata.Text.Length < 1
                || novaSobaBrojSobe.Text.Length < 1
                || novaSobaBrojKreveta.Text.Length < 1
                || NovaSobaImaTV.IsChecked.HasValue == false
                || novaSobaImaKlimu.IsChecked.HasValue == false
                || novaSobaCijenaNocenja.Text.Length < 1
                || novaSobaHotel.SelectedItem == null)
            {
                rezultatSobe.Text = "Popunite sva polja.";
                rezultatSobe.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatSobe.Text = "";

            try
            {
                HotelService hotelService = new HotelService();
                ComboBoxItem hotelItem = (ComboBoxItem)novaSobaHotel.SelectedItem;
                int hotelID = (int)hotelItem.Tag;
                Hotel hotel = await hotelService.GetById(hotelID);

                int brojSprata = int.Parse(novaSobaBrojSprata.Text);
                int brojSobe = int.Parse(novaSobaBrojSobe.Text);
                int brojKreveta = int.Parse(novaSobaBrojKreveta.Text);
                bool imaTV = NovaSobaImaTV.IsChecked.Value;
                bool imaKlimu = novaSobaImaKlimu.IsChecked.Value;
                double cijenaNocenja = double.Parse(novaSobaCijenaNocenja.Text);

                Soba soba = new Soba(0, brojSprata, brojSobe, brojKreveta, imaTV, imaKlimu, cijenaNocenja, hotel.HotelID);
                SobaService sobaService = new SobaService();

                if (await sobaService.Insert(soba) == -1)
                {
                    rezultatSobe.Text = "Neuspjesno kreiranje.";
                    rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    rezultatSobe.Text = "Soba kreirana.";
                    rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickSobe(object sender, RoutedEventArgs e)
        {
            SobaService sobaService = new SobaService();
            rezultatSobe.Text = "";

            int counter = 0;

            foreach (Soba s in sobe)
            {
                if (await sobaService.Update(s) != -1)
                {
                    counter++;
                }
            }

            rezultatSobe.Text = "Azurirani unosi.";
            rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickSobe(object sender, RoutedEventArgs e)
        {
            Soba selectedSoba = (Soba)sobeDataGrid.SelectedItem;
            SobaService sobaService = new SobaService();

            rezultatSobe.Text = "";

            if (selectedSoba == null)
            {
                rezultatSobe.Text = "Odaberite sobu.";
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await sobaService.GetAllReservations(selectedSoba.SobaID)).Count > 0)
            {
                rezultatSobe.Text = "Soba ima aranzmane.";
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await sobaService.Delete(selectedSoba.SobaID)) == -1)
            {
                rezultatSobe.Text = "Neuspjesno brisanje.";
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatSobe.Text = "Soba obrisana.";
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

                UpdateDataGrid();
            }
        }

        private async void AddClickGost(object sender, RoutedEventArgs e)
        {
            if (noviGostJMBG.Text.Length != 13
                || noviGostIme.Text.Length < 2
                || noviGostPrezime.Text.Length < 2)
            {
                rezultatGost.Text = "Popunite sva polja.";
                rezultatGost.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatGost.Text = "";

            try
            {
                string JMBG = noviGostJMBG.Text;
                string ime = noviGostIme.Text;
                string prezime = noviGostPrezime.Text;

                Gost gost = new Gost(0, JMBG, ime, prezime);
                GostService gostService = new GostService();

                if (await gostService.Insert(gost) == -1)
                {
                    rezultatGost.Text = "Neuspjesno kreiranje.";
                    rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    rezultatGost.Text = "Gost kreiran.";
                    rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickGost(object sender, RoutedEventArgs e)
        {
            GostService gostService = new GostService();
            rezultatGost.Text = "";

            int counter = 0;

            foreach (Gost g in gosti)
            {
                if (await gostService.Update(g) != -1)
                {
                    counter++;
                }
            }

            rezultatGost.Text = "Azurirani unosi.";
            rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickGost(object sender, RoutedEventArgs e)
        {
            Gost selectedGost = (Gost)gostiDataGrid.SelectedItem;
            GostService gostService = new GostService();

            rezultatGost.Text = "";

            if (selectedGost == null)
            {
                rezultatGost.Text = "Odaberite gosta.";
                rezultatGost.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await gostService.GetAllReservations(selectedGost.GostID)).Count > 0)
            {
                rezultatGost.Text = "Gost ima aranzmane.";
                rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await gostService.Delete(selectedGost.GostID)) == -1)
            {
                rezultatGost.Text = "Neuspjesno brisanje.";
                rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatGost.Text = "Gost obrisan.";
                rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

                UpdateDataGrid();
            }
        }
    }
}
