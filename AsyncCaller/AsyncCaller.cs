using System;
using System.Threading.Tasks;

namespace AsyncCaller
{
    public sealed class AsyncCaller
    {
        private readonly EventHandler _eventHandler;

        public AsyncCaller(EventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public bool Invoke(int miliSecondEnd, object sender, EventArgs eventArgs)
        {
            Task task = Task.Factory.StartNew(() => _eventHandler.Invoke(sender, eventArgs));
            return task.Wait(miliSecondEnd);
        }
    }
}