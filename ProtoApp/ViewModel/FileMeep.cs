using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ProtoApp.ViewModel
{
    public class FileMeep : Meep
    {
        public List<IRandomAccessStreamWithContentType> LocalFiles { get; set; } = new List<IRandomAccessStreamWithContentType>();

        
    }
}
