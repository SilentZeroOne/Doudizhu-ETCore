
using System;
using System.Collections.Generic;

namespace ETModel
{
    public class TimeTestComponent:Component
    {
        private Entity parent;
        private readonly Dictionary<string, ITimeBehaviour> Tbehaviours = new Dictionary<string, ITimeBehaviour>();

        public void Awake()
        {
            parent = GetParent<Entity>();
            Load();
        }
        
        public void Run(string type, long time = 0)
        {
            try
            {
                Tbehaviours[type].Behaviour(parent,time);
            }
            catch (Exception e)
            {
                throw new Exception($"{type} Time Behavior 错误: {e}");
            }
        }

        public void Load()
        {
            Tbehaviours.Clear();
            var types = Game.EventSystem.GetTypes(typeof(TimeBehaviourAttribute));
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(TimeBehaviourAttribute), false);
                if(attrs.Length==0) continue;
                var attribute = attrs[0] as TimeBehaviourAttribute;
                if (Tbehaviours.ContainsKey(attribute.Type))
                {
                    Log.Debug($"已经存在同类Time Behavior: {attribute.Type}");
                    throw new Exception($"已经存在同类Time Behavior: {attribute.Type}");
                }

                var o = Activator.CreateInstance(type);
                var behaviour = o as ITimeBehaviour;
                if (behaviour == null)
                {
                    Log.Error($"{o.GetType().FullName} 没有继承 ITimeBehavior");
                    continue;
                }

                Tbehaviours.Add(attribute.Type, behaviour);
            }
        }
    }

    [ObjectSystem]
    public class TimeTestAwakeSystem : AwakeSystem<TimeTestComponent>
    {
        public override void Awake(TimeTestComponent self)
        {
            self.Awake();
        }
    }
}