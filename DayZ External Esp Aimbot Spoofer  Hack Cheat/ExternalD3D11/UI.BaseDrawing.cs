using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace ExternalD3D11
{
    public partial class UI
    {
        #region Text

        public void DrawBaseText(TextFormat font, string text, float x, float y, Color color)
        {
            lock (drawTextLock)
            {
                d3dBase.DrawText(text, font, x, y, color);
            }
        }

        public void DrawBaseText(TextFormat font, string text, RectangleF rect, Color color)
        {
            lock (drawTextLock)
            {
                d3dBase.DrawTextRect(text, font, rect, color);
            }
        }

        public void DrawCenterText(TextFormat font, string text, RectangleF rect, Color color)
        {
            lock (drawTextLock)
            {
                d3dBase.DrawCenterText(font, text, rect, color);
            }
        }

        public void DrawShadowText(TextFormat font, string text, float x, float y, Color color)
        {
            lock (drawTextLock)
            {
                d3dBase.DrawShadowText(text, font, x, y, color);
            }
        }

        public Size2F MeasureText(string text, TextFormat font)
        {
            return d3dBase.MeasureText(text, font);
        }

        #endregion

        #region Boxes

        public void DrawBox(float x, float y, float w, float h, float strokeWidth, Color color)
        {
            lock (drawBoxLock)
            {
                d3dBase.DrawBox(x, y, w, h, strokeWidth, color);
            }
        }

        public void DrawFilledBox(float x, float y, float w, float h, Color color)
        {
            lock (drawFilledBoxLock)
            {
                d3dBase.DrawFilledBox(x, y, w, h, color);
            }
        }

        public void DrawTransparentBox(float x, float y, float w, float h, Color color, int transparency)
        {
            lock (drawFilledBoxLock)
            {
                color.A = (byte)transparency;
                DrawFilledBox(x, y, w, h, color);
            }
        }

        public void DrawBorderEdges(float x, float y, float w, float h, float strokeWidth, Color color)
        {
            d3dBase.DrawBorderEdges(x, y, w, h, strokeWidth, color);
        }

        #endregion

        #region Line

        public void DrawLine(float x1, float y1, float x2, float y2, float strokeWidth, Color color)
        {
            lock (drawLineLock)
            {
                d3dBase.DrawLine(x1, y1, x2, y2, strokeWidth, color);
            }
        }

        #endregion

        #region Circle

        public void DrawCircle(float centerX, float centerY, float radius, float strokeWidth, Color color)
        {
            lock (drawCircleLock)
            {
                d3dBase.DrawCircle(centerX, centerY, radius, strokeWidth, color);
            }
        }

        #endregion

        #region Bitmaps

        public void DrawBitmap(Bitmap bitmap, float opacity, RawRectangleF destinationRect, RawRectangleF sourceRectange)
        {
            d3dBase.DrawBitmap(bitmap, opacity, destinationRect, sourceRectange);
        }

        public Bitmap LoadImageFromFile(string file)
        {
            return d3dBase.LoadImageFromFile(file);
        }

        #endregion
    }
}
