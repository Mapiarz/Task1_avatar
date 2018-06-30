using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aisens
{
    public interface ISensorDetector
    {
        Task<IList<ISensorInfo>> DiscoverSensorsAsync( int timeout );
    }
}