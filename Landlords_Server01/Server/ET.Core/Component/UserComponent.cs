using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    public class UserComponent : Component
    {
        private readonly Dictionary<long, User> idUsers = new Dictionary<long, User>();

        public void Add(User user)
        {
            if (Get(user.UserID) == null)
            {
                idUsers.Add(user.UserID, user);
            }
            else
            {
                Log.Error("重复登录！");
            }
        }

        public User Get(long id)
        {
            idUsers.TryGetValue(id, out User user);
            return user;
        }

        public void Remove(long id)
        {
            idUsers.Remove(id);
        }

        public int Count => idUsers.Values.Count;

        public User[] GetAllUsers()
        {
            return idUsers.Values.ToArray();
        }

        public override void Dispose()
        {
            if (IsDisposed) return;

            base.Dispose();
            foreach (var user in idUsers.Values)
            {
                user.Dispose();
            }
        }
    }
}