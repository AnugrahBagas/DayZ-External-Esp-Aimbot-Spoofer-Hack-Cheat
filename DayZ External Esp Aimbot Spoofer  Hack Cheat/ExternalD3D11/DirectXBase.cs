using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FontFactory = SharpDX.DirectWrite.Factory;

namespace ExternalD3D11
{
    public class DirectXBase
    {
        #region Globals

        private WindowRenderTarget device;
        private SolidColorBrush defaultBrush;
        
        #endregion

        #region Events

        public delegate void OnOverlayLoadedEventHandler(object sender);
        public event OnOverlayLoadedEventHandler OnOverlayLoaded;

        #endregion

        #region Properties

        public D3DOverlay Overlay { get; set; }
        public FontFactory FontBase { get; set; }

        public RawMatrix3x2 DeviceTransform { get { return device.Transform; } set { device.Transform = value; } }

        #endregion

        #region Constructor

        public DirectXBase()
        {
            Overlay = new D3DOverlay();
            Overlay.OnOverlayLoaded += Overlay_OnOverlayLoaded;
            FontBase = Overlay.FontBase;
            device = Overlay.Device;
            defaultBrush = new SolidColorBrush(device, Color.Red);
        }

        private void Overlay_OnOverlayLoaded(object sender)
        {
            if (OnOverlayLoaded != null)
                OnOverlayLoaded(this);
        }

        #endregion

        #region Methods

        public void BeginDraw()
        {
            Overlay.BeginDraw();
        }

        public void EndDraw()
        {
            Overlay.EndDraw();
        }

        public void DrawText(string text, TextFormat font, float x, float y, Color color)
        {
            defaultBrush.Color = color;
            device.DrawText(text, font, new RectangleF(x, y, Overlay.Width, Overlay.Height), defaultBrush);
        }

        public void DrawTextRect(string text, TextFormat font, RectangleF rect, Color color)
        {
            defaultBrush.Color = color;
            device.DrawText(text, font, rect, defaultBrush);
        }

        public void DrawShadowText(string text, TextFormat font, float x, float y, Color color)
        {
            DrawText(text, font, x + 1, y + 1, Color.Black);
            DrawText(text, font, x, y, color);
        }

        public void DrawRotatedText(string text, TextFormat font, float x, float y, float angle, Color color)
        {
            RawMatrix3x2 oldTransform = device.Transform;

            device.Transform = Matrix3x2.Rotation(angle, new Vector2(x, y));
            DrawText(text, font, x, y, color);

            device.Transform = oldTransform;
        }

        public void DrawTextWithWrapping(string text, TextFormat font, float x, float y, float width, float height, Color color)
        {
            TextLayout layout = new TextLayout(FontBase, text, font, width, height);
            layout.WordWrapping = WordWrapping.Wrap;
            layout.ParagraphAlignment = ParagraphAlignment.Near;

            defaultBrush.Color = color;
            device.DrawTextLayout(new RawVector2(x, y), layout, defaultBrush);
            layout.Dispose();
        }

        public void DrawCenterText(TextFormat font, string text, RectangleF rect, Color color)
        {
            float centerX = rect.Width / 2;
            float centerY = rect.Height / 2;

            Size2F textSize = MeasureText(text, font);

            float x = (centerX - (textSize.Width / 2)) + rect.X;
            float y = (centerY - (textSize.Height / 2)) + rect.Y - 1;

            DrawText(text, font, x, y, color);
        }

        public Size2F MeasureText(string text, TextFormat font)
        {
            TextLayout layout = new TextLayout(FontBase, text, font, Overlay.Width, Overlay.Height);
            layout.ParagraphAlignment = ParagraphAlignment.Center;
            
            Size2F size = new Size2F(layout.Metrics.Width, layout.Metrics.Height);
            layout.Dispose();
            return size;
        }

        public void DrawBox(float x, float y, float w, float h, float strokeWidth, Color color)
        {
            w = x + w;
            h = y + h;

            defaultBrush.Color = color;
            device.DrawRectangle(new RawRectangleF(x, y, w, h), defaultBrush, strokeWidth);
        }

        public void DrawFilledBox(float x, float y, float w, float h, Color color)
        {
            w = x + w;
            h = y + h;

            defaultBrush.Color = color;
            device.FillRectangle(new RawRectangleF(x, y, w, h), defaultBrush);
        }

        public void DrawRoundedBox(float x, float y, float w, float h, float radius, Color color)
        {
            w = x + w;
            h = y + h;

            RoundedRectangle rectangle = new RoundedRectangle();
            rectangle.RadiusX = radius;
            rectangle.RadiusY = radius;
            rectangle.Rect = new RawRectangleF(x, y, w, h);

            defaultBrush.Color = color;
            device.DrawRoundedRectangle(rectangle, defaultBrush);
        }

        public void DrawFilledRoundedBox(float x, float y, float w, float h, float radius, Color color)
        {
            w = x + w;
            h = y + h;

            RoundedRectangle rectangle = new RoundedRectangle();
            rectangle.RadiusX = radius;
            rectangle.RadiusY = radius;
            rectangle.Rect = new RawRectangleF(x, y, w, h);

            defaultBrush.Color = color;
            device.FillRoundedRectangle(rectangle, defaultBrush);
        }

        public void DrawBorderEdges(float x, float y, float w, float h, float strokeWidth, Color color)
        {
            float topGapWidth = w / 2;
            float leftGapheight = h / 3;

            float horizontalLineLength = (topGapWidth / 2);
            float verticalLineLength = (leftGapheight / 2);


            float topLeftX = x;
            float topLeftY = y;

            float topRightX = x + w;
            float topRightY = y;

            float bottomRightX = x + w;
            float bottomRightY = y + h;

            float bottomLeftX = x;
            float bottomLeftY = y + h;


            // Top Left
            float line1X2 = x + horizontalLineLength;
            float line2Y2 = y + verticalLineLength;

            DrawLine(topLeftX, topLeftY, line1X2, topLeftY, strokeWidth, color);
            DrawLine(topLeftX, topLeftY, topLeftX, line2Y2, strokeWidth, color);


            // Top Right
            float line3X2 = topRightX - horizontalLineLength;
            float line4Y2 = y + verticalLineLength;

            DrawLine(topRightX, topLeftY, line3X2, topLeftY, strokeWidth, color);
            DrawLine(topRightX, topLeftY, topRightX, line4Y2, strokeWidth, color);


            // Bottom Left
            float line5Y2 = bottomRightY - verticalLineLength;
            float line6X2 = x + horizontalLineLength;

            DrawLine(topLeftX, bottomRightY, topLeftX, line5Y2, strokeWidth, color);
            DrawLine(topLeftX, bottomRightY, line6X2, bottomRightY, strokeWidth, color);


            // Bottom Right
            float line7X2 = topRightX - horizontalLineLength;
            float line8Y2 = bottomRightY - verticalLineLength;

            DrawLine(topRightX, bottomRightY, line7X2, bottomRightY, strokeWidth, color);
            DrawLine(topRightX, bottomRightY, topRightX, line8Y2, strokeWidth, color);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, float strokeWidth, Color color)
        {
            defaultBrush.Color = color;
            device.DrawLine(new RawVector2(x1, y1), new RawVector2(x2, y2), defaultBrush, strokeWidth);
        }

        public void DrawCircle(float centerX, float centerY, float radius, float strokeWidth, Color color)
        {
            defaultBrush.Color = color;
            device.DrawEllipse(new Ellipse(new RawVector2(centerX, centerY), radius, radius), defaultBrush, strokeWidth);
        }

        public void FillCirlce(float centerX, float centerY, float radius, Color color)
        {
            defaultBrush.Color = color;
            device.FillEllipse(new Ellipse(new RawVector2(centerX, centerY), radius, radius), defaultBrush);
        }

        public void DrawBitmap(Bitmap bitmap, float opacity, RawRectangleF destinationRect, RawRectangleF sourceRectange)
        {
            device.DrawBitmap(bitmap, destinationRect, opacity, BitmapInterpolationMode.Linear, sourceRectange);
        }

        public void Dispose()
        {
            device.Dispose();
            defaultBrush.Dispose();
            Overlay.Dispose();
        }


        public Bitmap LoadImageFromFile(string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(device, size, tempStream, stride, bitmapProperties);
                }
            }
        }


        #endregion
    }
}
