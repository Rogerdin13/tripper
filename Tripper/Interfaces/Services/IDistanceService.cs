using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.Interfaces.Services
{
    public interface IDistanceService
    {
        List<Position> GetPositions();
        bool AddPosition(Position position);
        /// <summary>
        ///     resets all tracked points and calculated all distances
        /// </summary>
        /// <returns></returns>
        bool ResetTrip();
        /// <summary>
        ///     resets only the partial distance
        /// </summary>
        /// <returns></returns>
        bool ResetPartialDistance();
        double TotalDistance();
        double PartialDistance();
    }
}
