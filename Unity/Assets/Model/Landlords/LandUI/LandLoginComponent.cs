using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel 
{
    [ObjectSystem]
    public class LandLoginComponentAwakeSystem : AwakeSystem<LandLoginComponent>
    {
        public override void Awake(LandLoginComponent self)
        {
            self.Awake();
        }
    }

    public class LandLoginComponent:Component
    {
        public Text prompt;
        public InputField account;
        public InputField password;

        public bool isLogining;
        public bool isRegistering;

        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            isLogining = false;
            isRegistering = false;

            rc.Get<GameObject>("LoginButton").GetComponent<Button>().onClick.AddListener(LoginBtnOnClick);
            rc.Get<GameObject>("RegisterButton").GetComponent<Button>().onClick.AddListener(RegisterBtnOnClick);
            
        }

        public void LoginBtnOnClick()
        {
            if (isLogining || IsDisposed) return;
            isLogining = true;
            LandHelper.Login(this, account.text, password.text).Coroutine();
        }

        public void RegisterBtnOnClick()
        {
            if (isRegistering || IsDisposed) return;
            isRegistering = true;
            LandHelper.Register(this, account.text, password.text).Coroutine();
        }
    }
}
