using System.Collections.Generic;
using System.Threading;

namespace ETModel
{
    public class TestRoom : Entity
    {
        public CancellationTokenSource waitCts;
        public CancellationTokenSource randCts;
        public Dictionary<int, string> gamers = new Dictionary<int, string>();
    }
}