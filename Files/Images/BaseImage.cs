﻿using ShenmueDKSharp.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Images
{
    /// <summary>
    /// Base image class which all image classes inherit from.
    /// </summary>
    /// <seealso cref="ShenmueDKSharp.Files.BaseFile" />
    public abstract class BaseImage : BaseFile
    {
        /// <summary>
        /// Width of the image.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the image.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Mipmaps pixel buffers.
        /// </summary>
        public List<MipMap> MipMaps { get; set; } = new List<MipMap>();
        /// <summary>
        /// True of the image has an transparency channel.
        /// </summary>
        public bool HasTransparency { get; set; }

        /// <summary>
        /// Size of the image in it's inherited format as bytes
        /// </summary>
        public abstract int DataSize { get; }

        /// <summary>
        /// Creates an GDI+ bitmap object of the given mipmap.
        /// </summary>
        public Bitmap CreateBitmap(int mipmap = 0)
        {
            if (mipmap >= MipMaps.Count || mipmap < 0)
            {
                throw new IndexOutOfRangeException("Mipmap index out of range!");
            }
            return MipMaps[mipmap].GetBitmap();
        }

    }

    public class MipMap
    {
        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>
        /// BGRA32 byte array.
        /// </summary>
        public byte[] Pixels { get; set; }

        public MipMap() { }
        /// <summary>
        /// Copy constructor.
        /// </summary>
        public MipMap(MipMap mipmap)
        {
            Width = mipmap.Width;
            Height = mipmap.Height;
            Pixels = new byte[Width * Height * 4];
            Array.Copy(mipmap.Pixels, Pixels, Pixels.Length);
        }
        public MipMap(byte[] data, int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new byte[Width * Height * 4];
            Array.Copy(data, Pixels, Pixels.Length);
        }
        public MipMap(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new byte[Width * Height * 4];
        }

        public Bitmap GetBitmap()
        {
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, Height),
                                                ImageLockMode.WriteOnly,
                                                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmp.Height;

            Marshal.Copy(Pixels, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public Color4[] GetPixelsAsColor4()
        {
            Color4[] result = new Color4[Pixels.Length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                Color4 color = result[i];
                color.B_ = Pixels[i * 4];
                color.G_ = Pixels[i * 4 + 1];
                color.R_ = Pixels[i * 4 + 2];
                color.A_ = Pixels[i * 4 + 3];
            }
            return result;
        }
    }
}
