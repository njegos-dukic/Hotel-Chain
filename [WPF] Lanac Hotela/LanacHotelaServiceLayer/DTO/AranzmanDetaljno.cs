using System;
using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class AranzmanDetaljno
    {
        public int HotelID { get; set; }
        public int AranzmanID { get; set; }
        public string HotelIme { get; set; }
        public string GostIme { get; set; }
        public string GostPrezime { get; set; }
        public string Pocetak { get; set; }
        public string Kraj { get; set; }
        public bool Otkazan { get; set; }
        public bool Zavrsen { get; set; }
        public int SobaID { get; set; }
        public double CijenaNocenja { get; set; }
        public int UkupnoNocenja { get; set; }

        public AranzmanDetaljno(int hotelID, int aranzmanID, string hotelIme, string gostIme, string gostPrezime, DateTime pocetak, DateTime kraj, bool otkazan, bool zavrsen, int sobaID, double cijenaNocenja, int ukupnoNocenja)
        {
            HotelID = hotelID;
            AranzmanID = aranzmanID;
            HotelIme = hotelIme;
            GostIme = gostIme;
            GostPrezime = gostPrezime;
            Pocetak = pocetak.ToString("dd-MMM-yyyy"); ;
            Kraj = kraj.ToString("dd-MMM-yyyy"); ;
            Otkazan = otkazan;
            Zavrsen = zavrsen;
            SobaID = sobaID;
            CijenaNocenja = cijenaNocenja;
            UkupnoNocenja = ukupnoNocenja;
        }

        public override bool Equals(object obj)
        {
            return obj is AranzmanDetaljno detaljno &&
                   HotelIme == detaljno.HotelIme &&
                   GostIme == detaljno.GostIme &&
                   GostPrezime == detaljno.GostPrezime &&
                   Pocetak == detaljno.Pocetak &&
                   Kraj == detaljno.Kraj &&
                   Otkazan == detaljno.Otkazan &&
                   Zavrsen == detaljno.Zavrsen &&
                   SobaID == detaljno.SobaID &&
                   CijenaNocenja == detaljno.CijenaNocenja &&
                   UkupnoNocenja == detaljno.UkupnoNocenja;
        }

        public override int GetHashCode()
        {
            int hashCode = -9356911;
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            hashCode = hashCode * -1521134295 + AranzmanID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HotelIme);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GostIme);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GostPrezime);
            hashCode = hashCode * -1521134295 + Pocetak.GetHashCode();
            hashCode = hashCode * -1521134295 + Kraj.GetHashCode();
            hashCode = hashCode * -1521134295 + Zavrsen.GetHashCode();
            hashCode = hashCode * -1521134295 + Otkazan.GetHashCode();
            hashCode = hashCode * -1521134295 + SobaID.GetHashCode();
            hashCode = hashCode * -1521134295 + CijenaNocenja.GetHashCode();
            hashCode = hashCode * -1521134295 + UkupnoNocenja.GetHashCode();
            return hashCode;
        }
    }
}
