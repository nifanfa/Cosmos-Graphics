﻿using System;
using System.Drawing;
using System.IO;
using System.Text;
using Cosmos.Core;
using Cosmos.System.Graphics;

namespace CosmosKernel1
{
    unsafe class Graphics
    {
        public int[] raw;
        public int width, height;
        public int* address;
        const int bytesPerPixel = 4;
        static int transparent = Color.Transparent.ToArgb();

        static string ASC16Base64 = "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AAB4zMzM2MzGxsbMAAAAAAAA/sbGwMDAwMDAwAAAAAAAAAAA/mxsbGxsbGwAAAAAAAAA/sZgMBgwYMb+AAAAAAAAAAAAftjY2NjYcAAAAAAAAAAAZmZmZmZ8YGDAAAAAAAAAAHbcGBgYGBgYAAAAAAAAAH4YPGZmZjwYfgAAAAAAAAA4bMbG/sbGbDgAAAAAAAA4bMbGxmxsbGzuAAAAAAAAHjAYDD5mZmZmPAAAAAAAAAAAAH7b29t+AAAAAAAAAAAAAwZ+29vzfmDAAAAAAAAAHDBgYHxgYGAwHAAAAAAAAAB8xsbGxsbGxsYAAAAAAAAAAP4AAP4AAP4AAAAAAAAAAAAYGH4YGAAA/wAAAAAAAAAwGAwGDBgwAH4AAAAAAAAADBgwYDAYDAB+AAAAAAAADhsbGBgYGBgYGBgYGBgYGBgYGBgYGNjY2HAAAAAAAAAAABgYAH4AGBgAAAAAAAAAAAAAdtwAdtwAAAAAAAAAOGxsOAAAAAAAAAAAAAAAAAAAAAAAABgYAAAAAAAAAAAAAAAAAAAAGAAAAAAAAAAADwwMDAwM7GxsPBwAAAAAANhsbGxsbAAAAAAAAAAAAABw2DBgyPgAAAAAAAAAAAAAAAAAfHx8fHx8fAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        static MemoryStream ASC16FontMS = new MemoryStream(Convert.FromBase64String(ASC16Base64));

        public Graphics(int width, int height)
        {
            raw = new int[width * height];
            fixed (int* pointer = raw)
            {
                address = pointer;
            }
            this.width = width;
            this.height = height;
        }

        public void Clear(int color)
        {
            MemoryOperations.Fill((byte*)address, color, raw.Length * bytesPerPixel);
        }

        public void DrawImage(int x, int y, Image image)
        {
            fixed (int* imageRawAddress = image.rawData)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    if (x >= 0)
                    {
                        int w = (int)image.Width;
                        if (x + image.Width > width)
                        {
                            w = width - x;
                        }
                        MemoryOperations.Copy(address + (width * (h + y)) + x, imageRawAddress + (image.Width * h), w);
                    }
                    else
                    {
                        if (image.Width + x >= 0)
                        {
                            MemoryOperations.Copy(address + (width * (h + y)), imageRawAddress + (image.Width * h + x), (int)image.Width + x);
                        }
                    }
                }
            }
        }

        public void DrawImageWithAlpha(int x, int y, Image image)
        {
            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    //可能会导致错误，65536>>8 = 256，比255多1。
                    //No Colorspace

                    Color foreground = Color.FromArgb(image.rawData[image.Width * h + w]);
                    Color background = Color.FromArgb(GetPixel(x + w, y + h));

                    int alpha = foreground.A;
                    int inv_alpha = 255 - alpha;

                    int newR = (foreground.R * alpha + inv_alpha * background.R) >> 8;
                    int newG = (foreground.G * alpha + inv_alpha * background.G) >> 8;
                    int newB = (foreground.B * alpha + inv_alpha * background.B) >> 8;

                    Color newColor = Color.FromArgb(newR, newB, newG);

                    if (foreground.A != 0)
                    {
                        SetPixel(x + w, y + h, newColor.ToArgb());
                    }
                }
            }
        }

        public void DrawMyImage(string path)
        {
        }

        public void SetPixel(int x, int y, int color)
        {
            if (x >= 0 && x <= width && y >= 0 && y <= height)
            {
                raw[width * y + x] = color;
            }
        }

        public int GetPixel(int x, int y)
        {
            if (x >= 0 && x <= width && y >= 0 && y <= height)
            {
                return raw[width * y + x];
            }
            return transparent;
        }

        public void CopyFromGraphics(Graphics graphics, int x, int y)
        {
            for (int h = 0; h < height; h++)
            {
                int w = width;
                if (graphics.width - x > width)
                {
                    w = graphics.width - x;
                }
                MemoryOperations.Copy(address + width * h, graphics.address + graphics.width * (h + y) + x, w);
            }
        }

        public void Blur(int intensity)
        {
            int[] _raw = raw;
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    long r = 0, g = 0, b = 0, a = 0;
                    int counter = 0;

                    for (int ww = w - intensity; ww < w + intensity; ww++)
                    {
                        for (int hh = h - intensity; hh < h + intensity; hh++)
                        {
                            if (ww >= 0 && hh >= 0 && ww < width && hh < height)
                            {
                                Color color = Color.FromArgb(_raw[width * hh + ww]);

                                r += color.R;
                                g += color.G;
                                b += color.B;
                                a += color.A;

                                counter++;
                            }
                        }
                    }

                    r /= counter;
                    g /= counter;
                    b /= counter;
                    a /= counter;

                    SetPixel(w, h, Color.FromArgb((int)a, (int)r, (int)g, (int)b).ToArgb());
                }
            }
        }

        public void DrawACSIIString(int x, int y, string s, int color)
        {
            string[] lines = s.Split('\n');
            for (int l = 0; l < lines.Length; l++)
            {
                for (int c = 0; c < lines[l].Length; c++)
                {
                    int offset = (Encoding.ASCII.GetBytes(lines[l][c].ToString())[0] & 0xFF) * 16;
                    ASC16FontMS.Seek(offset, SeekOrigin.Begin);
                    byte[] fontbuf = new byte[16];
                    ASC16FontMS.Read(fontbuf, 0, fontbuf.Length);

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if ((fontbuf[i] & (0x80 >> j)) != 0)
                            {
                                SetPixel((x + j) + (c * 8), y + i + (l * 16), color);
                            }
                        }
                    }
                }
            }
        }

        public void Fill(int x, int y, int width, int height, int color)
        {
            for (int h = 0; h < height; h++)
            {
                if (x >= 0)
                {
                    int w = width;
                    if (x + width > this.width)
                    {
                        w = this.width - x;
                    }
                    MemoryOperations.Fill(address + (this.width * (h + y)) + x, color, w);
                }
                else
                {
                    if (width + x >= 0)
                    {
                        MemoryOperations.Fill(address + (this.width * (h + y)), color, width + x);
                    }
                }
            }
        }

        public void Flush(int x, int y, uint videoWidth, uint videoHeight, MemoryBlock videoMemory)
        {
            for (int h = 0; h < height; h++)
            {
                int offset = (int)((y + h) * videoWidth * bytesPerPixel + x * bytesPerPixel);
                int w = width;
                if (x + w > videoWidth)
                {
                    w = (int)(videoWidth - x);
                }
                videoMemory.Copy(offset, raw, width * h, w);
            }
        }

        public void DrawCircle(int x_center, int y_center, int radius, int color)
        {
            /*
            ThrowIfCoordNotValid(x_center + radius, y_center);
            ThrowIfCoordNotValid(x_center - radius, y_center);
            ThrowIfCoordNotValid(x_center, y_center + radius);
            ThrowIfCoordNotValid(x_center, y_center - radius);
            */
            int x = radius;
            int y = 0;
            int e = 0;

            while (x >= y)
            {
                SetPixel(x_center + x, y_center + y, color);
                SetPixel(x_center + y, y_center + x, color);
                SetPixel(x_center - y, y_center + x, color);
                SetPixel(x_center - x, y_center + y, color);
                SetPixel(x_center - x, y_center - y, color);
                SetPixel(x_center - y, y_center - x, color);
                SetPixel(x_center + y, y_center - x, color);
                SetPixel(x_center + x, y_center - y, color);

                y++;
                if (e <= 0)
                {
                    e += 2 * y + 1;
                }
                if (e > 0)
                {
                    x--;
                    e -= 2 * x + 1;
                }
            }
        }

        public void DrawRectangle(int x, int y, int width, int height, int color)
        {
            /*
             * we must draw four lines connecting any vertex of our rectangle to do this we first obtain the position of these
             * vertex (we call these vertexes A, B, C, D as for geometric convention)
             */
            /* The check of the validity of x and y are done in DrawLine() */

            /* The vertex A is where x,y are */
            int xa = x;
            int ya = y;

            /* The vertex B has the same y coordinate of A but x is moved of width pixels */
            int xb = x + width;
            int yb = y;

            /* The vertex C has the same x coordiate of A but this time is y that is moved of height pixels */
            int xc = x;
            int yc = y + height;

            /* The Vertex D has x moved of width pixels and y moved of height pixels */
            int xd = x + width;
            int yd = y + height;

            /* Draw a line betwen A and B */
            DrawLine(xa, ya, xb, yb, color);

            /* Draw a line between A and C */
            DrawLine(xa, ya, xc, yc, color);

            /* Draw a line between B and D */
            DrawLine(xb, yb, xd, yd, color);

            /* Draw a line between C and D */
            //DrawLine(xc, yc, xd, yd, color);
            DrawLine(xc, yc, xd + 1, yd, color);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            // trim the given line to fit inside the canvas boundries
            TrimLine(ref x1, ref y1, ref x2, ref y2);

            int dx, dy;

            dx = x2 - x1;      /* the horizontal distance of the line */
            dy = y2 - y1;      /* the vertical distance of the line */

            if (dy == 0) /* The line is horizontal */
            {
                DrawHorizontalLine(color, dx, x1, y1);

                /*
                int minx = Math.Min(x1, x2);
                int maxx = Math.Max(x1, x2);

                for (int i = minx; i < maxx; i++)
                {
                    DrawPoint(i, y1, color);
                }
                */

                return;
            }

            if (dx == 0) /* the line is vertical */
            {
                DrawVerticalLine(color, dy, x1, y1);

                /*
                int miny = Math.Min(y1, y2);
                int maxy = Math.Max(y1, y2);

                for (int i = miny; i < maxy; i++)
                {
                    DrawPoint(x1, i, color);
                }
                */

                return;
            }

            /* the line is neither horizontal neither vertical, is diagonal then! */
            DrawDiagonalLine(color, dx, dy, x1, y1);
        }

        #region DrawLine
        private void DrawVerticalLine(int color, int dy, int x1, int y1)
        {
            int i;

            for (i = 0; i < dy; i++)
            {
                SetPixel(x1, (y1 + i), color);
            }
        }

        private void DrawHorizontalLine(int color, int dx, int x1, int y1)
        {
            uint i;

            for (i = 0; i < dx; i++)
            {
                SetPixel(((int)(x1 + i)), y1, color);
            }
        }

        protected void TrimLine(ref int x1, ref int y1, ref int x2, ref int y2)
        {
            // in case of vertical lines, no need to perform complex operations
            if (x1 == x2)
            {
                x1 = Math.Min(width - 1, Math.Max(0, x1));
                x2 = x1;
                y1 = Math.Min(height - 1, Math.Max(0, y1));
                y2 = Math.Min(height - 1, Math.Max(0, y2));

                return;
            }

            // never attempt to remove this part,
            // if we didn't calculate our new values as floats, we would end up with inaccurate output
            float x1_out = x1, y1_out = y1;
            float x2_out = x2, y2_out = y2;

            // calculate the line slope, and the entercepted part of the y axis
            float m = (y2_out - y1_out) / (x2_out - x1_out);
            float c = y1_out - m * x1_out;

            // handle x1
            if (x1_out < 0)
            {
                x1_out = 0;
                y1_out = c;
            }
            else if (x1_out >= width)
            {
                x1_out = width - 1;
                y1_out = (width - 1) * m + c;
            }

            // handle x2
            if (x2_out < 0)
            {
                x2_out = 0;
                y2_out = c;
            }
            else if (x2_out >= width)
            {
                x2_out = width - 1;
                y2_out = (width - 1) * m + c;
            }

            // handle y1
            if (y1_out < 0)
            {
                x1_out = -c / m;
                y1_out = 0;
            }
            else if (y1_out >= height)
            {
                x1_out = (height - 1 - c) / m;
                y1_out = height - 1;
            }

            // handle y2
            if (y2_out < 0)
            {
                x2_out = -c / m;
                y2_out = 0;
            }
            else if (y2_out >= height)
            {
                x2_out = (height - 1 - c) / m;
                y2_out = height - 1;
            }

            // final check, to avoid lines that are totally outside bounds
            if (x1_out < 0 || x1_out >= width || y1_out < 0 || y1_out >= height)
            {
                x1_out = 0; x2_out = 0;
                y1_out = 0; y2_out = 0;
            }

            if (x2_out < 0 || x2_out >= width || y2_out < 0 || y2_out >= height)
            {
                x1_out = 0; x2_out = 0;
                y1_out = 0; y2_out = 0;
            }

            // replace inputs with new values
            x1 = (int)x1_out; y1 = (int)y1_out;
            x2 = (int)x2_out; y2 = (int)y2_out;
        }

        private void DrawDiagonalLine(int color, int dx, int dy, int x1, int y1)
        {
            int i, sdx, sdy, dxabs, dyabs, x, y, px, py;

            dxabs = Math.Abs(dx);
            dyabs = Math.Abs(dy);
            sdx = Math.Sign(dx);
            sdy = Math.Sign(dy);
            x = dyabs >> 1;
            y = dxabs >> 1;
            px = x1;
            py = y1;

            if (dxabs >= dyabs) /* the line is more horizontal than vertical */
            {
                for (i = 0; i < dxabs; i++)
                {
                    y += dyabs;
                    if (y >= dxabs)
                    {
                        y -= dxabs;
                        py += sdy;
                    }
                    px += sdx;
                    SetPixel(px, py, color);
                }
            }
            else /* the line is more vertical than horizontal */
            {
                for (i = 0; i < dyabs; i++)
                {
                    x += dxabs;
                    if (x >= dyabs)
                    {
                        x -= dyabs;
                        px += sdx;
                    }
                    py += sdy;
                    SetPixel(px, py, color);
                }
            }
        }
        #endregion
    }
}
