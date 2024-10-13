using System.Drawing;

namespace MyDomain.Models
{
    public class ImageConverter
    {

        public byte[] FilePNGToByteArray(String pathName)
        {
            Image imageIn = Image.FromFile(pathName);
            using (var ms = new MemoryStream())
            {

                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                return ms.ToArray();
            }
        }

        public Image? ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) { return null; }
            using (var ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = Image.FromStream(ms);

                return returnImage;
            }
        }
    }
}
