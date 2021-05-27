using System;
using System.Threading;

namespace ETModel
{
    [TimeBehaviour(Typebehaviour.RandTarget)]
    public class RandTargetTimeBehaviour:ITimeBehaviour
    {
        public TestRoom room;
        public Random rd = new Random();
        public int count = 0;
        public int maxCount;
        public void Behaviour(Entity parent, long time)
        {
            room = parent as TestRoom;
            StartRand();
        }

        private void StartRand()
        {
            room.gamers.Add(1,"gamer01");
            room.gamers.Add(2,"gamer02");
            room.gamers.Add(3,"gamer03");
            room.gamers.Add(4,"gamer04");
            room.gamers.Add(5,"gamer05");

            maxCount = rd.Next(2, 4);
            Log.Info($"将要点名{maxCount}次");
            RandTimeAndTarget().Coroutine();
        }

        private async ETVoid RandTimeAndTarget()
        {
            int num = rd.Next(1, 5);
            string target = room.gamers[num];
            int randTime = rd.Next(3, 12);

            var timer = Game.Scene.GetComponent<TimerComponent>();
            room.randCts = new CancellationTokenSource();

            await timer.WaitAsync((randTime + 1) * 1000, room.randCts.Token);
            
            Log.Info($"{room.GetType().ToString()}-执行间隔{randTime}秒点名{target}");
            room.randCts.Dispose();
            room.randCts = null;

            count++;
            if(count<maxCount)
                RandTimeAndTarget().Coroutine();
        }
    }
}