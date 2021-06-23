namespace LanacHotelaServiceLayer
{
    internal interface ICrudTemplate<T> : IReadable<T>, IUniquelyReadable<T>, IInsertable<T>, IUniquelyUpdateable<T>, IUniquelyDeleteable
    {
    }
}
