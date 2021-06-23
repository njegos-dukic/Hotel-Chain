using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class Kontakt
    {
        public int KontaktID { get; set; }
        public string Tip { get; set; }
        public string Info { get; set; }
        public int HotelID { get; set; }
        public int GostID { get; set; }

        public Kontakt(int kontaktID, string tip, string info, int hotelID, int gostID)
        {
            KontaktID = kontaktID;
            Tip = tip;
            Info = info;
            HotelID = hotelID;
            GostID = gostID;
        }

        public override bool Equals(object obj)
        {
            return obj is Kontakt kontakt &&
                   Tip.ToLower() == kontakt.Tip.ToLower() &&
                   Info.ToLower() == kontakt.Info.ToLower() &&
                   HotelID == kontakt.HotelID &&
                   GostID == kontakt.GostID;
        }

        public override int GetHashCode()
        {
            int hashCode = 247409742;
            hashCode = hashCode * -1521134295 + KontaktID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tip);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Info);
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            hashCode = hashCode * -1521134295 + GostID.GetHashCode();
            return hashCode;
        }
    }
}
