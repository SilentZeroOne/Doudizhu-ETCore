using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class FrameTestComponentUpdateSystem : UpdateSystem<FrameTestComponent>
    {
        public override void Update(FrameTestComponent self)
        {
            self.Update();
        }
    }

    public class FrameTestComponent :Component
    {
        public int waitTime = 1000;
        public bool interval = false;
        public int count = 1;
        public void Update()
        {
            if (interval)
            {
                return;
            }
            UpdateAsync().Coroutine();
        }

        public async ETVoid UpdateAsync()
        {
            Log.Info("Frame count " + count);
            interval = true;
            count++;

            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();
            await timer.WaitAsync(waitTime);
            interval = false;
        }
    }
}
