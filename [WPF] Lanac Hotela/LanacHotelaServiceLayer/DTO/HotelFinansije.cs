using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class HotelFinansije
    {
        public int HotelID { get; set; }
        public string ImeHotela { get; set; }
        public int UkupnoNocenja { get; set; }
        public double UkupnaZarada { get; set; }

        public HotelFinansije(int hotelID, string imeHotela, int ukupnoNocenja, double ukupnaZarada)
        {
            HotelID = hotelID;
            ImeHotela = imeHotela;
            UkupnoNocenja = ukupnoNocenja;
            UkupnaZarada = ukupnaZarada;
        }

        public override bool Equals(object obj)
        {
            return obj is HotelFinansije finansije &&
                   ImeHotela == finansije.ImeHotela &&
                   UkupnoNocenja == finansije.UkupnoNocenja &&
                   UkupnaZarada == finansije.UkupnaZarada;
        }

        public override int GetHashCode()
        {
            int hashCode = -1075031824;
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ImeHotela);
            hashCode = hashCode * -1521134295 + UkupnoNocenja.GetHashCode();
            hashCode = hashCode * -1521134295 + UkupnaZarada.GetHashCode();
            return hashCode;
        }
    }
}
