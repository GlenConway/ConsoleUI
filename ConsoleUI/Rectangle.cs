namespace ConsoleUI
{
    public class Rectangle : Control
    {
        protected override void DrawControl()
        {
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

        protected override void DrawBorder()
        {
            base.DrawBorder();
        }
    }
}