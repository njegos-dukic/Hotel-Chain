using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IUniquelyUpdateable<T>
    {
        public Task<int> Update(T t);
    }
}
