using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_LoginHandler: AMRpcHandler<C2R_Login,R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            try
            {
                var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                var result =
                    await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}',Password:'{request.Password}'}}");

                if (result.Count != 1)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply();
                    return;
                }

                var account = (AccountInfo) result[0];
                int gateAppid;
                StartConfig config;
                //获取账号所在区服的AppId 索取登陆Key
                if (StartConfigComponent.Instance.GateConfigs.Count == 1)
                {//只有一个Gate服务器时当作AllServer配置处理
                    config = StartConfigComponent.Instance.StartConfig;
                }
                else
                {//有多个Gate服务器时当作分布式配置处理
                    gateAppid = RealmHelper.GetGateAppIdFromUserId(account.Id);
                    config = StartConfigComponent.Instance.GateConfigs[gateAppid - 1];
                }

                var innderAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innderAddress);
                var outerAddress = config.GetComponent<OuterConfig>().Address2;
                var g2RGetLoginKey =
                    (G2R_GetLoginKey) await gateSession.Call(new R2G_GetLoginKey() {UserID = account.Id});
                response.Key = g2RGetLoginKey.Key;
                response.Address = outerAddress;
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response,e,reply);
            }
            
        }
    }
}