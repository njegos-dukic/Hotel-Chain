using System;

namespace LanacHotelaServiceLayer
{
    public class Aranzman
    {
        public int AranzmanID { get; set; }
        public DateTime Pocetak { get; set; }
        public DateTime Kraj { get; set; }
        public bool JeOtkazan { get; set; }
        public bool JeZavrsen { get; set; }
        public int HotelID { get; set; }
        public int GostID { get; set; }
        public int SobaID { get; set; }

        public Aranzman(int aranzmanID, DateTime pocetak, DateTime kraj, bool jeOtkazan, bool jeZavrsen, int hotelID, int gostID, int sobaID)
        {
            AranzmanID = aranzmanID;
            Pocetak = pocetak;
            Kraj = kraj;
            JeOtkazan = jeOtkazan;
            JeZavrsen = jeZavrsen;
            HotelID = hotelID;
            GostID = gostID;
            SobaID = sobaID;
        }

        public override bool Equals(object obj)
        {
            return obj is Aranzman aranzman &&
                   Pocetak == aranzman.Pocetak &&
                   Kraj == aranzman.Kraj &&
                   JeOtkazan == aranzman.JeOtkazan &&
                   JeZavrsen == aranzman.JeZavrsen &&
                   HotelID == aranzman.HotelID &&
                   GostID == aranzman.GostID &&
                   SobaID == aranzman.SobaID;
        }

        public override int GetHashCode()
        {
            int hashCode = -1913838358;
            hashCode = hashCode * -1521134295 + AranzmanID.GetHashCode();
            hashCode = hashCode * -1521134295 + Pocetak.GetHashCode();
            hashCode = hashCode * -1521134295 + Kraj.GetHashCode();
            hashCode = hashCode * -1521134295 + JeOtkazan.GetHashCode();
            hashCode = hashCode * -1521134295 + JeZavrsen.GetHashCode();
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            hashCode = hashCode * -1521134295 + GostID.GetHashCode();
            hashCode = hashCode * -1521134295 + SobaID.GetHashCode();
            return hashCode;
        }
    }
}
