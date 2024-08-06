using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Client.BuildFeature
{
    public abstract class State<T>
    {
        public T StateObject;

        public virtual void Tick() { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public virtual async Task OnStateEnterAsync() { }

        public virtual async Task OnStateExitAsync() { }

        public Action OnClickAction { get; set; }

        public State(T stateObject)
        {
            this.StateObject = stateObject;
        }
    }
}