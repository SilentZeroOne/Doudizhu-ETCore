using System.Collections.Generic;

namespace ETModel
{
    public class SessionKeyComponent : Component
    {
        private readonly Dictionary<long, long> sessionKeys = new Dictionary<long, long>();

        public void Add(long key, long userId)
        {
            sessionKeys.Add(key, userId);
        }
        
        public long Get(long key)
        {
            sessionKeys.TryGetValue(key, out long userId);
            return userId;
        }

        public void Remove(long key)
        {
            sessionKeys.Remove(key);
        }

        private async void TimeoutRemoveKey(long key)
        {
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
            sessionKeys.Remove(key);
        }
    }
}