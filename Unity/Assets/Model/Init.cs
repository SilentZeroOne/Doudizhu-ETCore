using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			this.StartAsync().Coroutine();
		}
		
		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				ClientConfigHelper.SetConfigHelper();
				Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
				Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

				Game.Scene.AddComponent<GlobalConfigComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();
				Game.Scene.AddComponent<TimerComponent>();

				//Test One
				Game.Scene.AddComponent<OpcodeTestComponent>();
				//Test two
				Game.Scene.AddComponent<FrameTestComponent>();
				//Test Three
				TestRoom room = ComponentFactory.Create<TestRoom>();
				var timeTest = room.AddComponent<TimeTestComponent>();
				timeTest.Run(Typebehaviour.Waiting,5000);
				

				Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
				
				UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
				Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");


				//UI
				Game.Scene.AddComponent<UIComponent>();

				//Gamer
				Game.Scene.AddComponent<GamerComponent>();

				//消息分发组件
				Game.Scene.AddComponent<MessageDispatcherComponent>();
				

				//添加指定与网络组件
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				var netOuter = Game.Scene.AddComponent<NetOuterComponent>();

				Game.EventSystem.Run(UIEventType.LandInitSceneStart);

				//测试发送给服务端一条文本消息
				Session session = netOuter.Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                G2C_TestMessage g2CTestMessage = (G2C_TestMessage) await session.Call(new C2G_TestMessage() { Info = "==>>服务端的朋友,你好!收到请回答" });

                session = netOuter.Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                var g2CMyMessage = (G2C_MyMessage) await session.Call(new C2G_MyMessage() {MyInfo = "==>>Fuck you"});
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}