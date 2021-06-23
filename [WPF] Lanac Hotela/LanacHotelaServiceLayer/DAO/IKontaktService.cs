using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IKontaktService : IUniquelyReadable<Kontakt>, IReadable<Kontakt>, IInsertable<Kontakt>, IUniquelyUpdateable<Kontakt>, IUniquelyDeleteable
    {
        Task<List<Kontakt>> GetAllUnassigned();
        Task<int> Update(Kontakt t, int hotelID);
    }
}
