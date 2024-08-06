namespace Assets.Scripts
{
    public class NoUpdateState<T>
    {
        protected T StateObject;

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public NoUpdateState(T stateObject)
        {
            StateObject = stateObject;
        }
    }
}
