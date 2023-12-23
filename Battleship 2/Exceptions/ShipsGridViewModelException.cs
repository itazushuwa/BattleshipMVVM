using System;

namespace Battleship_2.Exceptions
{
    internal class ShipsGridViewModelException : Exception
    {
        public ShipsGridViewModelException(string message) : base(message) { }
    }
}
