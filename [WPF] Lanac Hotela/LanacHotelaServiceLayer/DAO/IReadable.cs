using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IReadable<T>
    {
        public Task<List<T>> GetAll();
    }
}
