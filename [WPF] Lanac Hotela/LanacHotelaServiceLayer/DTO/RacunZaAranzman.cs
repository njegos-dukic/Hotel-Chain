using System;
using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class RacunZaAranzman
    {
        public int RacunAranzmanID { get; set; }
        public int AranzmanID { get; set; }
        public double Cijena { get; set; }

        public RacunZaAranzman(int racunAranzmanID, int aranzmanID, double cijena)
        {
            RacunAranzmanID = racunAranzmanID;
            AranzmanID = aranzmanID;
            Cijena = cijena;
        }

        public override bool Equals(object obj)
        {
            return obj is RacunZaAranzman aranzman &&
                   AranzmanID == aranzman.AranzmanID &&
                   Cijena == aranzman.Cijena;
        }

        public override int GetHashCode()
        {
            int hashCode = -1017055856;
            hashCode = hashCode * -1521134295 + RacunAranzmanID.GetHashCode();
            hashCode = hashCode * -1521134295 + AranzmanID.GetHashCode();
            hashCode = hashCode * -1521134295 + Cijena.GetHashCode();
            return hashCode;
        }

        public List<RacunZaAranzman> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
