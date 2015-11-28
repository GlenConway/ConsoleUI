using System;
using System.Timers;

namespace ConsoleUI
{
    public class ProgressBar : Control
    {
        public ConsoleColor BlockColor = ConsoleColor.White;
        private int marqueeEnd;
        private int marqueeStart;
        private ProgressBarStyle progressBarStyle;
        private Timer timer;

        private int value;

        public ProgressBar()
        {
            Minimum = 0;
            Maximum = 100;
            Value = 0;
            Height = 1;

            timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
        }

        public int Maximum { get; set; }

        public int Minimum { get; set; }

        public ProgressBarStyle ProgressBarStyle
        {
            get
            {
                return progressBarStyle;
            }
            set
            {
                if (progressBarStyle != value)
                {
                    progressBarStyle = value;

                    if (value == ProgressBarStyle.Marquee)
                        timer.Start();
                    else
                        timer.Stop();
                }
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;

                    DrawControl();
                    Paint();
                }
            }
        }

        private double Percent
        {
            get
            {
                if (Value == 0)
                    return 0;

                if (Maximum == 0)
                    return 0;

                var range = Maximum - Minimum;

                if (range == 0)
                    return 0;

                return ((double)Value / (double)range);
            }
        }

        public void Increment(int value)
        {
            Value += value;

            if (Value > Maximum)
                Value = Maximum;

            if (Value < Minimum)
                Value = Minimum;
        }

        protected override void DrawControl()
        {
            if (!Visible)
                return;

            if (ProgressBarStyle == ProgressBarStyle.Blocks)
                DrawBlocks();
        }

        private void DrawBlocks()
        {
            var position = (int)(ClientWidth * Percent);

            for (int i = 0; i < ClientWidth; i++)
            {
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, i <= position ? (byte)219 : (byte)32, BlockColor, BackgroundColor);
            }
        }

        private void DrawMarquee()
        {
            if (!Owner.Visible)
                return;

            if (!Visible)
                return;

            timer.Stop();

            marqueeEnd += 5;

            if (marqueeEnd > 100)
                marqueeEnd = 100;

            if (marqueeStart < (marqueeEnd - 20) || marqueeEnd == 100)
                marqueeStart += 5;

            if (marqueeStart > 100)
            {
                marqueeStart = 0;
                marqueeEnd = 0;
            }

            var position1 = (int)(ClientWidth * ((double)marqueeStart / 100));
            var position2 = (int)(ClientWidth * ((double)marqueeEnd / 100));

            for (int i = 0; i < ClientWidth; i++)
            {
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, (i >= position1 & i <= position2) ? (byte)219 : (byte)32, BlockColor, BackgroundColor);
            }

            Paint();

            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DrawMarquee();
        }
    }
}