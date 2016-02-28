using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ProtoApp
{
    public class StorageFileToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(ImageSource))
            {
                var file = value as IRandomAccessStreamWithContentType;
                if (file != null)
                {
                    var image = new BitmapImage();
                    image.SetSource( file );

                    

                    return image;
                }
            }

            throw new NotSupportedException("StorageFile can not be converted to an Image!");
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
