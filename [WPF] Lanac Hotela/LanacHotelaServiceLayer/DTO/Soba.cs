namespace LanacHotelaServiceLayer
{
    public class Soba
    {
        public int SobaID { get; set; }
        public int BrojSprata { get; set; }
        public int BrojSobe { get; set; }
        public int BrojKreveta { get; set; }
        public bool ImaTV { get; set; }
        public bool ImaKlimu { get; set; }
        public double CijenaNocenja { get; set; }
        public int HotelID { get; set; }

        public Soba(int sobaID, int brojSprata, int brojSobe, int brojKreveta, bool imaTV, bool imaKlimu, double cijenaNocenja, int hotelID)
        {
            SobaID = sobaID;
            BrojSprata = brojSprata;
            BrojSobe = brojSobe;
            BrojKreveta = brojKreveta;
            ImaTV = imaTV;
            ImaKlimu = imaKlimu;
            CijenaNocenja = cijenaNocenja;
            HotelID = hotelID;
        }

        public override bool Equals(object obj)
        {
            return obj is Soba soba &&
                   BrojSprata == soba.BrojSprata &&
                   BrojSobe == soba.BrojSobe &&
                   BrojKreveta == soba.BrojKreveta &&
                   ImaTV == soba.ImaTV &&
                   ImaKlimu == soba.ImaKlimu &&
                   CijenaNocenja == soba.CijenaNocenja &&
                   HotelID == soba.HotelID;
        }

        public override int GetHashCode()
        {
            int hashCode = 1646709440;
            hashCode = hashCode * -1521134295 + SobaID.GetHashCode();
            hashCode = hashCode * -1521134295 + BrojSprata.GetHashCode();
            hashCode = hashCode * -1521134295 + BrojSobe.GetHashCode();
            hashCode = hashCode * -1521134295 + BrojKreveta.GetHashCode();
            hashCode = hashCode * -1521134295 + ImaTV.GetHashCode();
            hashCode = hashCode * -1521134295 + ImaKlimu.GetHashCode();
            hashCode = hashCode * -1521134295 + CijenaNocenja.GetHashCode();
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            return hashCode;
        }
    }
}
