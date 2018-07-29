using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using ImageMagick;

namespace ChannelMerger
{
    static class Helpers
    {
        public static string ImagesFilter()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string filter = string.Format("{0}|All image files ({1})|{1}|All files|*", 
                string.Join("|", codecs.Select(codec => 
                    string.Format("{0} ({1})|{1}", codec.CodecName.Split(' ')[1], codec.FilenameExtension)).ToArray()),
                string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));

            return filter;
        }

        public static MagickImage MergeChannels(this MagickImageCollection images, string path, string name)
        {
            IMagickImage result = images.Combine();
            result.Write(path + "/" + name);
            Process.Start(result.FileName);
            var img = new MagickImage(result.FileName);
            return img;
        }

        public static bool CheckGrayscale(this MagickImage i)
        {
            using (MagickImage image = i)
            {
                // -colorspace HSL
                image.ColorSpace = ColorSpace.HSL;

                // -channel g -separate +channel
                string r;
                using (IMagickImage channel = image.Separate(Channels.Gray).First())
                {
                    // -format "%[fx:mean]"
                    r = channel.FormatExpression("%[fx:mean]");
                }

                return double.Parse(r, NumberStyles.Any, CultureInfo.InvariantCulture) < 1e-10;
            }
        }
    }
}
