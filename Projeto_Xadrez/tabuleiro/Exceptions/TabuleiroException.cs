using System;

namespace tabuleiro.Exceptions
{
    class TabuleiroException : Exception
    { 

        public TabuleiroException(string message)
            : base(message)
        {
        }

    }
}
