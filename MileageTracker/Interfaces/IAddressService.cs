using MileageTracker.Models;
using MileageTracker.ViewModels;

namespace MileageTracker.Interfaces {
    public interface IAddressService {
        PaginationViewModel GetAddressesPaginated(int pageNumber, int pageSize);
        Address GetAddressById(int id);
        Address AddAddress(Address address);
        Address UpdateAddress(Address address);
        bool DeleteAddress(int id);
    }
}
