using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IAranzmanService : IUniquelyReadable<Aranzman>, IReadable<Aranzman>, IInsertable<Aranzman>, IUniquelyDeleteable, IUniquelyUpdateable<AranzmanDetaljno>
    {
        Task<List<Aranzman>> GetAllForHotel(int hotelID);
        Task<List<Aranzman>> GetAllForGuest(int gostID);
    }
}
