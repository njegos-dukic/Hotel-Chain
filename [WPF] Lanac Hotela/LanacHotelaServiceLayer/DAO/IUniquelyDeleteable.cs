using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    internal interface IUniquelyDeleteable
    {
        public Task<int> Delete(int id);
    }
}
