using System.Collections.Generic;
using System.Threading.Tasks;
using TeachRed.Domain.Response;
using TeachRed.Domain.ViewModels;

namespace TeachRed.Service.Interfaces
{
    public interface IServiceService
    {
        Task<BaseResponse<List<ServiceViewModel>>> GetAllServices();
    }
}