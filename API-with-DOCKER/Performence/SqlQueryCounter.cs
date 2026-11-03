
using System.Threading;

namespace API_with_DOCKER.Performance
{
    public sealed class SqlQueryCounter
    {
        private int _count = 0;
        public int Count => Volatile.Read(ref _count);
        public void Increment()
        {
            Interlocked.Increment(ref _count);
        }
    }
}
