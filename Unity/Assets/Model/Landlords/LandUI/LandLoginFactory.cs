using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [UIFactory(LandUIType.LandLogin)]
    public class LandLoginFactory : IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");

                GameObject bundleGameobject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject landObject = UnityEngine.Object.Instantiate(bundleGameobject);

                landObject.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(landObject);

                ui.AddComponent<LandLoginComponent>();
                return ui;
            }
            catch(Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
    }
}
