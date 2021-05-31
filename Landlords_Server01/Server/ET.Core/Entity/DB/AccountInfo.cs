using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class AccountInfo : Entity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
