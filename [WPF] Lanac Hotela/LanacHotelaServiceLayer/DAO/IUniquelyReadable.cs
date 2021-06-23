using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IUniquelyReadable<T>
    {
        public Task<T> GetById(int id);
    }
}
