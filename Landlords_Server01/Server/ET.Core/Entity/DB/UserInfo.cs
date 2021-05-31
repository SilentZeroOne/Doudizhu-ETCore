using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class UserInfoAwakeSystem : AwakeSystem<UserInfo, string>
    {
        public override void Awake(UserInfo self, string name)
        {
            self.Awake(name);
        }
    }

    public class UserInfo : Entity
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 玩家等级
        /// </summary>
        public int Level { get; set; }

        public long Money { get; set; }


        /// <summary>
        /// 上次游戏角色序列
        /// </summary>
        public int LastPlay { get; set; }

        public void Awake(string name)
        {
            UserName = name;
            Level = 1;
            Money = 10000;
            LastPlay = 0;
        }
    }
}
