﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Models._OBJ
{
    /// <summary>
    /// Wavefront OBJ format
    /// </summary>
    public class MTL : BaseFile
    {
        BaseModel Model;

        public MTL(BaseModel model)
        {
            Model = model;
        }
        public MTL(string filepath)
        {
            Read(filepath);
        }
        public MTL(Stream stream)
        {
            Read(stream);
        }
        public MTL(StreamReader reader)
        {
            Read(reader);
        }

        public override void Read(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Read(reader);
            }
        }

        public override void Write(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                Write(writer);
            }
        }

        public void Read(StreamReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(StreamWriter writer)
        {
            writer.Write("# MTL Generated by ShenmueDKSharp\n");
            for (int i = 0; i < Model.Textures.Count; i++)
            {
                Texture texture = Model.Textures[i];

                writer.Write(String.Format("newmtl mat_{0}\n", i));
                writer.Write("Ka 1.000 1.000 1.000\n");
                writer.Write("Kd 1.000 1.000 1.000\n");
                writer.Write("Ks 0.000 0.000 0.000\n");
                writer.Write("d 1.0\n");
                writer.Write("illum 2\n");

                string textureName = String.Format("tex_{0}.bmp", i);
                if (String.IsNullOrEmpty(FilePath))
                {
                    //TODO: Make this somehow better
                    throw new ArgumentException("Filepath was not given.");
                }
                string texturePath = String.Format("{0}\\{1}", Path.GetDirectoryName(FilePath), textureName);
                string texDir = Path.GetDirectoryName(texturePath);
                if (!Directory.Exists(texDir))
                {
                    Directory.CreateDirectory(texDir);
                }

                Bitmap bmp = texture.Image.CreateBitmap();
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(texturePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bmp.Save(memory, ImageFormat.Bmp);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                writer.Write(String.Format("map_Kd {0}\n\n", textureName));
            }
        }

    }
}
