using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class Hotel
    {
        public int HotelID { get; set; }
        public string Ime { get; set; }
        public int BrojZvjezdica { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Grad { get; set; }
        public int PostanskiBroj { get; set; }
        public string Drzava { get; set; }

        public Hotel(int hotelID, string ime, int brojZvjezdica, string ulica, string broj, string grad, int postanskiBroj, string drzava)
        {
            HotelID = hotelID;
            Ime = ime;
            BrojZvjezdica = brojZvjezdica;
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            PostanskiBroj = postanskiBroj;
            Drzava = drzava;
        }

        public override bool Equals(object obj)
        {
            return obj is Hotel hotel &&
                   Ime.ToLower() == hotel.Ime.ToLower() &&
                   BrojZvjezdica == hotel.BrojZvjezdica &&
                   Ulica.ToLower() == hotel.Ulica.ToLower() &&
                   Broj.ToLower() == hotel.Broj.ToLower() &&
                   Grad.ToLower() == hotel.Grad.ToLower() &&
                   PostanskiBroj == hotel.PostanskiBroj &&
                   Drzava.ToLower() == hotel.Drzava.ToLower();
        }

        public override int GetHashCode()
        {
            int hashCode = -250469685;
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Ime);
            hashCode = hashCode * -1521134295 + BrojZvjezdica.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Ulica);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Broj);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Grad);
            hashCode = hashCode * -1521134295 + PostanskiBroj.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Drzava);
            return hashCode;
        }
    }
}
