using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace AIO
{

    /*
    bitmap文件的
    第3-8位存儲了文件大小信息，
    第19-22位存儲了高度信息，
    第23-26位存儲了寬度信息。
    文件頭後面都是像素的argb
    第二張圖像的像素argb放到第一張後面，
    並修改第一張的文件頭信息
    */
    class bmp
    {
        //設置文件頭裏面文件大小信息
        private static void SetBitmapFileSizeInfo(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long le = fileInfo.Length;
            string hexSize = le.ToString("X").PadLeft(8, '0');
            int size1 = Convert.ToInt32(hexSize.Substring(0, 2), 16);
            int size2 = Convert.ToInt32(hexSize.Substring(2, 2), 16);
            int size3 = Convert.ToInt32(hexSize.Substring(4, 2), 16);
            int size4 = Convert.ToInt32(hexSize.Substring(6, 2), 16);
            byte[] sizeBytes = new byte[] { (byte)size4, (byte)size3, (byte)size2, (byte)size1 };
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                using (BinaryWriter r = new BinaryWriter(fs))
                {
                    r.Seek(2, 0);
                    r.Write(sizeBytes, 0, sizeBytes.Length);
                }
            }
        }


        //設置文件頭裏面文件長度和寬度信息
        private static void SetBitmapSizeInfo(string filePath, int width = 0, int height = 0)
        {
            if (height != 0)
            {
                string hexHeight = height.ToString("X").PadLeft(8, '0');
                int h1 = Convert.ToInt32(hexHeight.Substring(0, 2), 16);
                int h2 = Convert.ToInt32(hexHeight.Substring(2, 2), 16);
                int h3 = Convert.ToInt32(hexHeight.Substring(4, 2), 16);
                int h4 = Convert.ToInt32(hexHeight.Substring(6, 2), 16);
                byte[] sizeHeight = new byte[] { (byte)h4, (byte)h3, (byte)h2, (byte)h1 };
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    using (BinaryWriter r = new BinaryWriter(fs))
                    {
                        r.Seek(22, 0);//高度保存位置
                        r.Write(sizeHeight, 0, sizeHeight.Length);
                    }
                }
            }
            if (width != 0)
            {
                string hexWidth = height.ToString("X").PadLeft(8, '0');
                int w1 = Convert.ToInt32(hexWidth.Substring(0, 2), 16);
                int w2 = Convert.ToInt32(hexWidth.Substring(2, 2), 16);
                int w3 = Convert.ToInt32(hexWidth.Substring(4, 2), 16);
                int w4 = Convert.ToInt32(hexWidth.Substring(6, 2), 16);
                byte[] sizeWidth = new byte[] { (byte)w4, (byte)w3, (byte)w2, (byte)w1 };
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    using (BinaryWriter r = new BinaryWriter(fs))
                    {
                        r.Seek(18, 0);//高度保存位置
                        r.Write(sizeWidth, 0, sizeWidth.Length);
                    }
                }
            }
        }

        //合併多個bitmap文件,並生成一個最終文件
        public static void CreateBitMap(string tempPath, string imagePath)
        {
            string[] files = Directory.GetFiles(tempPath, "*.bmp");
            Bitmap bmp;
            int height = 0;
            for (int i = files.Length - 1; i > 0; i--)
            {
                string fileName = files[i];
                bmp = new Bitmap(fileName);
                if (i == files.Length - 1)
                {
                    bmp.Save(imagePath, ImageFormat.Bmp);
                    height += bmp.Height;
                    bmp.Dispose();
                    continue;
                }
                else
                {
                    byte[] bytes = GetImageRasterBytes(bmp, PixelFormat.Format32bppRgb);
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Write))
                    {
                        fs.Seek(fs.Length, 0);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    height += bmp.Height;
                    bmp.Dispose();
                }
            }
            SetBitmapFileSizeInfo(imagePath);
            SetBitmapSizeInfo(imagePath, height: height);
            //MessageBox.Show("合併成功");
        }
        private static byte[] GetImageRasterBytes(Bitmap bmp, PixelFormat format)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            byte[] bits = null;
            try
            {
                // Lock the managed memory
                BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);
                // Declare an array to hold the bytes of the bitmap.
                bits = new byte[bmpdata.Stride * bmpdata.Height];
                // Copy the values into the array.
                System.Runtime.InteropServices.Marshal.Copy(bmpdata.Scan0, bits, 0, bits.Length);
                // Release managed memory
                bmp.UnlockBits(bmpdata);
            }
            catch
            {
                return null;
            }
            return bits;
        }


        //水平合併
        public static Image HorizontalMergeImages(Image img1, Image img2)
        {
            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            Wide = img1.Width + img2.Width;//設定寬度           
            if (img1.Height >= img2.Height)
            {
                High = img1.Height;
            }
            else
            {
                High = img2.Height;
            }
            Bitmap mybmp = new Bitmap(Wide, High);
            Graphics gr = Graphics.FromImage(mybmp);
            //處理第一張圖片
            gr.DrawImage(img1, 0, 0);
            //處理第二張圖片
            gr.DrawImage(img2, img1.Width, 0);
            MergedImage = mybmp;
            gr.Dispose();
            return MergedImage;
        }

        public static Image MergeImages(Image[,] img)
        {
            //假設有一 3x2 陣列，(三層樓，每一層樓有2戶)
            //int[,] arr1 = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            //GetLength方法--取得某一維度的長度(即元素個數)
            //GetLength(0)->得到第1維長度，是3(列數、樓層數)
            //GetLength(1)->得到第2維長度，是2(行數、戶數)
            //for (int i = 0; i <= arr1.GetLength(0) - 1; i++)
            //{
            //    for (int j = 0; j <= arr1.GetLength(1) - 1; j++)
            //    {
            //        Response.Write(arr1[i, j].ToString());
            //    }
            //}

            Image MergedImage = default(Image);
            int width = 0;
            int height = 0;
            int i;

            //Wide = img1.Width + img2.Width;//設定寬度           
            for (i = 0; i < img.GetLength(1); i++) { width += img[0, i].Width; }
            for (i = 0; i < img.GetLength(0); i++) { height += img[i, 0].Height; }

            Bitmap mybmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(mybmp);





            int jh = 0;          
            for (int j = 0; j < img.GetLength(0); j++)
            {
                int jw = 0;
                gr.DrawImage(img[j, 0], 0, jh);
                for (i = 1; i < img.GetLength(1); i++)
                {
                    jw += img[j, i - 1].Width;
                    gr.DrawImage(img[j, i], jw, jh);
                }
                jh += img[j, 0].Height;
            }

            ////處理第一張圖片
            //gr.DrawImage(img1, 0, 0);
            ////處理第二張圖片
            //gr.DrawImage(img2, img1.Width, 0);
            MergedImage = mybmp;
            gr.Dispose();
            return MergedImage;
        }

        //垂直合併
        public static Image VerticalMergeImages(Image img1, Image img2)
        {
            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            High = img1.Height + img2.Height;//設定高度          
            if (img1.Width >= img2.Width)
            {
                Wide = img1.Width;
            }
            else
            {
                Wide = img2.Width;
            }
            Bitmap mybmp = new Bitmap(Wide, High);
            Graphics gr = Graphics.FromImage(mybmp);
            //處理第一張圖片
            gr.DrawImage(img1, 0, 0);
            //處理第二張圖片
            gr.DrawImage(img2, 0, img1.Height);
            MergedImage = mybmp;
            mybmp.Save(mvars.strStartUpPath + @"\Parameter\coding.bmp");
            gr.Dispose();
            return MergedImage;
        }

        //圖片浮水印
        public static Image MarkImage(Image img1, Image img2)
        {
            Image MergedImage = default(Image);
            //設定背景圖片
            Graphics gr = System.Drawing.Graphics.FromImage(img1);
            //新建logo浮水印圖片
            Bitmap Logo = new Bitmap(img2.Width, img2.Height);
            Graphics tgr = Graphics.FromImage(Logo);
            ColorMatrix cmatrix = new ColorMatrix();
            //設定圖片色彩(透明度)
            cmatrix.Matrix33 = 0.5F;
            ImageAttributes imgattributes = new ImageAttributes();
            imgattributes.SetColorMatrix(cmatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            tgr.DrawImage(img2, new Rectangle(0, 0, Logo.Width, Logo.Height), 0, 0, Logo.Width, Logo.Height, GraphicsUnit.Pixel, imgattributes);
            tgr.Dispose();
            //logo圖片位置
            gr.DrawImage(Logo, img1.Width / 3, 10);
            gr.Dispose();
            MergedImage = img1;
            return MergedImage;
        }


    }
}
