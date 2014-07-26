using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    public interface IBusNetwork
    {
        //list of all available Tursib buses
        IEnumerable<Bus> Buses { get; }
    }
}
