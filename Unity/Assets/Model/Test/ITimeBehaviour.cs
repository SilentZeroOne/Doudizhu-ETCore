namespace ETModel
{
    public class TimeBehaviourAttribute : BaseAttribute
    {
        public  string Type { get; }

        public TimeBehaviourAttribute(string type)
        {
            Type = type;
        }
    }

    public partial class Typebehaviour
    {
        public const string Waiting = "Waitting";
        public const string RandTarget = "RandTarget";
    }
    
    public interface ITimeBehaviour
    {
        void Behaviour(Entity parent, long time);
    }
}