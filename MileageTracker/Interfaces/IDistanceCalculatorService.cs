using MileageTracker.Infrastructure.DistanceCalculation;
using MileageTracker.Models;

namespace MileageTracker.Interfaces {
    public interface IDistanceCalculatorService {
        int GetDistance(Address origin, Address destination, Mode mode, Units units);
    }
}
