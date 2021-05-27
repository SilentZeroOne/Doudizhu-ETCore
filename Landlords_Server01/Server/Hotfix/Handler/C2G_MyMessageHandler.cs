using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix.Handler
{
    [MessageHandler(AppType.Gate)]
    class C2G_MyMessageHandler : AMRpcHandler<C2G_MyMessage, G2C_MyMessage>
    {
        protected override async ETTask Run(Session session, C2G_MyMessage request, G2C_MyMessage response, Action reply)
        {
            response.Message = "<<== FUCK YOU!!";
            reply();
        }
    }
}
