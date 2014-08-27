using MileageTracker.Models;
using MileageTracker.ViewModels;

namespace MileageTracker.Interfaces {
    public interface ICarService {
        PaginationViewModel GetCarsPaginated(int pageNumber, int pageSize);
        Car GetCarById(int id);
        Car AddCar(Car car);
        Car UpdateCar(Car car);
        bool DeleteCar(int id);
    }
}
