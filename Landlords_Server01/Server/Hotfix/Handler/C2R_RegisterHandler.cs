using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_RegisterHandler: AMRpcHandler<C2R_Register,R2C_Register>
    {
        protected override async ETTask Run(Session session, C2R_Register request, R2C_Register response, Action reply)
        {
            try
            {
                var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                var result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}'}}");
                if (result.Count == 1)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegisted;
                    reply();
                    return;
                }
                else if (result.Count > 1)
                {
                    response.Error = ErrorCode.ERR_RepeatedAccountExist;
                    Log.Error("出现重复账号：" + request.Account);
                    reply();
                    return;
                }

                //生成玩家帐号 这里随机生成区号
                var newAccount = ComponentFactory.CreateWithId<AccountInfo>(RealmHelper.GenerateId());
                newAccount.Account = request.Account;
                newAccount.Password = request.Password;
                await dbProxy.Save(newAccount);

                var newUser = ComponentFactory.CreateWithId<UserInfo, string>(newAccount.Id, request.Account);
                await dbProxy.Save(newUser);

                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}