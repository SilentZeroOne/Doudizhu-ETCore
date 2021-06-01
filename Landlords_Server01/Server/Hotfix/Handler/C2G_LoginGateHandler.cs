using System;
using ETModel;

namespace ETHotfix.Handler
{
    [MessageHandler(AppType.Gate)]
    public class C2G_LoginGateHandler: AMRpcHandler<C2G_LoginGate,G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            try
            {
                var sessionKeyComponent = Game.Scene.GetComponent<SessionKeyComponent>();
                //获取玩家永久ID
                var gateUserID = sessionKeyComponent.Get(request.Key);
                if (gateUserID == 0)
                {
                    response.Error = ErrorCode.ERR_ConnectGateKeyError;
                    reply();
                    return;
                }

                //Key 过期
                sessionKeyComponent.Remove(request.Key);

                //gateUserID传参创建User
                var user = ComponentFactory.Create<User, long>(gateUserID);

                //将新上线的User添加到UserComponent容器中
                Game.Scene.GetComponent<UserComponent>().Add(user);
                user.AddComponent<MailBoxComponent>();

                //session挂SessionUser组件，user绑定到session上
                //session挂MailBox组件可以通过MailBox进行actor通信
                session.AddComponent<SessionUserComponent>().User = user;
                session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);
                var config = Game.Scene.GetComponent<StartConfigComponent>();
                //构建realmSession通知Realm服务器 玩家已上线
                //...

                //设置User的参数
                user.GateAppID = config.StartConfig.AppId;
                user.GateSessionID = session.InstanceId;
                user.ActorID = 0;

                response.PlayerId = user.UserID;
                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response,e,reply);
            }
        }
    }
}