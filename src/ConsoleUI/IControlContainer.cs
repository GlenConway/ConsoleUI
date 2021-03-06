﻿namespace ConsoleUI
{
    public interface IControlContainer
    {
        Buffer Buffer { get; }
        bool Visible { get; }

        void Paint();

        void SuspendLayout();
        void ResumeLayout();
    }
}