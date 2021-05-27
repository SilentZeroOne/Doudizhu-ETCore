using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class GamerComponentAwakeSystem : AwakeSystem<GamerComponent>
    {
        public override void Awake(GamerComponent self)
        {
            self.Awake();
        }
    }

    public class GamerComponent : Component
    {
        public static GamerComponent Instance { get; private set; }

        private readonly Dictionary<long, Gamer> idGamers = new Dictionary<long, Gamer>();

        public void Awake()
        {
            Instance = this;
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;

            base.Dispose();

            foreach (var gamer in idGamers.Values)
            {
                gamer.Dispose();
            }

            idGamers.Clear();
            Instance = null;
        }

        public void Add(Gamer gamer)
        {
            idGamers.Add(gamer.UserID, gamer);
        }

        public Gamer Get(long userid)
        {
            idGamers.TryGetValue(userid, out Gamer gamer);
            return gamer;
        }

        public void Remove(long userid)
        {
            idGamers.TryGetValue(userid, out Gamer gamer);
            idGamers.Remove(userid);
            gamer?.Dispose();
        }

        public void RemoveNoDispose(long userid)
        {
            idGamers.Remove(userid);
        }

        public int Count
        {
            get
            {
                return idGamers.Count;
            }
        }

        public Gamer[] GetAll()
        {
            return idGamers.Values.ToArray();
        }
    }
}

