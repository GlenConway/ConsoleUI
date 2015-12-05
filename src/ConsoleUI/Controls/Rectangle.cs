namespace ConsoleUI
{
    public class Rectangle : Control
    {
        protected override void DrawBorder()
        {
            base.DrawBorder();
        }

        protected override void DrawControl()
        {
            if (Owner == null)
                return;

            var x = ClientLeft;

            while (x <= ClientRight)
            {
                var y = ClientTop;

                while (y <= ClientBottom)
                {
                    Owner.Buffer.Write((short)x, (short)y, 32, ForegroundColor, BackgroundColor);

                    y++;
                }

                x++;
            }
        }
    }
}