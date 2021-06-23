using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class Gost
    {
        public int GostID { get; set; }
        public string JMBG { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public Gost(int gostID, string JMBG, string ime, string prezime)
        {
            GostID = gostID;
            this.JMBG = JMBG;
            Ime = ime;
            Prezime = prezime;
        }

        public override bool Equals(object obj)
        {
            return obj is Gost gost &&
                   JMBG == gost.JMBG &&
                   Ime == gost.Ime &&
                   Prezime == gost.Prezime;
        }

        public override int GetHashCode()
        {
            int hashCode = -1935510369;
            hashCode = hashCode * -1521134295 + GostID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(JMBG);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Ime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Prezime);
            return hashCode;
        }
    }
}
