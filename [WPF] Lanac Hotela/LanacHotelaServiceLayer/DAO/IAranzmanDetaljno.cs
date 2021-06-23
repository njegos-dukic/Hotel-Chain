using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IAranzmanDetaljno : IReadable<AranzmanDetaljno>
    {
        public Task<List<AranzmanDetaljno>> GetAllForHotel(int hotelID);
    }
}
