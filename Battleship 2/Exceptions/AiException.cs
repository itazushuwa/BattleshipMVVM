using System;

namespace Battleship_2.Exceptions
{
    internal class AiException : Exception
    {
        public AiException(string message) : base(message) { }
    }
}
