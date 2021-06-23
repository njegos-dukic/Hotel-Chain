using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IInsertable<T>
    {
        public Task<int> Insert(T t);
    }
}
