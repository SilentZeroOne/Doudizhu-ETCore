using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class OpcodeTestComponent :Component
    {
        public void Awake()
        {
            Log.Info("OpTest Awake...");
        }

        public void Load()
        {
            Log.Info("OpTest Loaded...");
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
    }

    [ObjectSystem]
    public class OpcodeTestComponentAwakeSystem : AwakeSystem<OpcodeTestComponent>
    {
        public override void Awake(OpcodeTestComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class OpCodeTestComponentLoadSystem : LoadSystem<OpcodeTestComponent>
    {
        public override void Load(OpcodeTestComponent self)
        {
            self.Load();
        }
    }
}
