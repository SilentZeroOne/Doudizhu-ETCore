using System.Threading;

namespace ETModel
{
    [TimeBehaviour(Typebehaviour.Waiting)]
    public class WaitingTimeBehaviour : ITimeBehaviour
    {
        public TestRoom room;
        public long waitTime;

        public void Behaviour(Entity parent, long time)
        {
            room = parent as TestRoom;
            waitTime = time;
            Waiting();
        }

        public async ETVoid Waiting()
        {
            var timer = Game.Scene.GetComponent<TimerComponent>();
            room.waitCts=new CancellationTokenSource();

            await timer.WaitAsync(waitTime,room.waitCts.Token);
            Log.Info($"{room.GetType().ToString()}-执行完waiting");
            room.waitCts.Dispose();
            room.waitCts = null;
        }
    }
}