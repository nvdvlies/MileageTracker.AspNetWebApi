using MileageTracker.Models;
using MileageTracker.ViewModels;

namespace MileageTracker.Interfaces {
    public interface ITripService {
        PaginationViewModel GetTripsPaginated(int pageNumber, int pageSize);
        Trip GetTripById(int id);
        Trip GetNewTripTemplate();
        Trip AddTrip(Trip trip);
        Trip UpdateTrip(Trip trip);
        bool DeleteTrip(int id);
    }
}
