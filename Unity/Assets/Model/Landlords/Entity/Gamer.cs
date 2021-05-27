using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public sealed class Gamer : Entity
    {
        public long UserID { get; private set; }

        public void Awake(long userid)
        {
            UserID = userid;
        }

        public Vector3 Position
        {
            get
            {
                return GameObject.transform.position;
            }
            set
            {
                GameObject.transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return GameObject.transform.rotation;
            }
            set
            {
                GameObject.transform.rotation = value;
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;

            base.Dispose();
        }
    }
}

