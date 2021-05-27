using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User, long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }

    public sealed class User : Entity
    {
        //用户ID（唯一）
        public long UserID { get; private set; }

        public void Awake(long id)
        {
            UserID = id;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            UserID = 0;
        }
    }
}
