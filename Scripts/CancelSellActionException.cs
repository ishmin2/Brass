using System;

namespace Assets.Scripts
{
    public class CancelSellActionException : Exception
    {
        public CancelSellActionException(string message) : base(message)
        {

        }
    }
}
