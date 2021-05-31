using ETModel;

namespace ETHotfix
{
    public static class GateHelper
    {
        public static bool SignSession(Session session)
        {
            var sessionUser = session.GetComponent<SessionUserComponent>();
            if (sessionUser == null || Game.Scene.GetComponent<UserComponent>().Get(sessionUser.User.UserID) == null)
            {
                return false;
            }
            return true;
        }
    }
}