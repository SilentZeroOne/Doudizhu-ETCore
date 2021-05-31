using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public static class LandHelper
    {
        public static async ETVoid Login(LandLoginComponent landLogin, string account,string password)
        {
            
            var netOuter = Game.Scene.GetComponent<NetOuterComponent>();
            //创建Realm Session
            var sessionRealm = netOuter.Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            var messageRealm =
                (R2C_Login) await sessionRealm.Call(new C2R_Login() {Account = account, Password = password});
            sessionRealm.Dispose();
            
            landLogin.prompt.text = "正在登陆中...";
            //判断服务器返回结果
            if (messageRealm.Error == ErrorCode.ERR_AccountOrPasswordError)
            {
                landLogin.prompt.text = "账号或密码错误！";
                landLogin.password.text = "";
                landLogin.isLogining = false;
                return;
            }
            
            //创建网关Session
            var sessionGate = netOuter.Create(messageRealm.Address);
            if (SessionComponent.Instance == null)
            {
                //Log.Debug("创建唯一Session");
                Game.Scene.AddComponent<SessionComponent>().Session = sessionGate;
            }
            else
            {
                //存入SessionComponent方便我们随时使用
                SessionComponent.Instance.Session = sessionGate;
            }
            var messageGate = (G2C_LoginGate) await sessionGate.Call(new C2G_LoginGate() {Key = messageRealm.Key});
            if (messageGate.Error == ErrorCode.ERR_ConnectGateKeyError)
            {
                landLogin.prompt.text = "连接网关服务器超时！请重试";
                landLogin.isLogining = false;
                sessionGate.Dispose();
                return;
            }
            //登陆Gate成功
            landLogin.prompt.text = "";
            var user = ComponentFactory.Create<User, long>(messageGate.PlayerId);
            GamerComponent.Instance.MyUser = user;
            
            Game.EventSystem.Run(UIEventType.LandLoginFinish);
        }

        public static async ETVoid Register(LandLoginComponent landLogin, string account, string password)
        {
            var netOuter = Game.Scene.GetComponent<NetOuterComponent>();
            //创建Realm Session
            var sessionRealm = netOuter.Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            var messageRealm =
                (R2C_Register) await sessionRealm.Call(new C2R_Register() {Account = account, Password = password});

            landLogin.isRegistering = false;
            if (messageRealm.Error == ErrorCode.ERR_AccountAlreadyRegisted)
            {
                landLogin.prompt.text = "注册失败，账户已被注册！";
                landLogin.password.text = "";
                landLogin.account.text = "";
                return;
            }
            
            if (messageRealm.Error == ErrorCode.ERR_RepeatedAccountExist)
            {
                landLogin.prompt.text = "注册失败，出现重复账户！";
                landLogin.password.text = "";
                landLogin.account.text = "";
                return;
            }

            landLogin.prompt.text = "注册成功！";
        }
    }
}
