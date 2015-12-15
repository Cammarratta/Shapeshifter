﻿namespace Shapeshifter.WindowsDesktop.Infrastructure.Logging.Interfaces
{
    using Dependencies.Interfaces;

    using Handles.Interfaces;

    interface ILogger: ISingleInstance
    {
        void Information(string text, int importanceFactor = 0);

        void Warning(string text);

        void Error(string text);

        void Performance(string text);

        IIndentationHandle Indent();
    }
}