using System.Collections.Generic;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Retrieves a list of all the buses currently in service by tursib.
    /// This includes all bus information, such as station names, etc.
    /// </summary>
    public interface IBusNetwork
    {
        IEnumerable<Bus> Buses { get; }
    }
}