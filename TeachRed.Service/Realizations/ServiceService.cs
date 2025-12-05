using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeachRed.Domain.Enum;
using TeachRed.Domain.Response;
using TeachRed.Domain.ViewModels;
using TeachRed.Service.Interfaces;
using TechReq.DAL.Interfaces;
using TechReq.Domain.ModelsDb;

namespace TeachRed.Service.Realizations
{
    public class ServiceService : IServiceService
    {
        private readonly IBaseStorage<ServiceDb> _serviceStorage;

        public ServiceService(IBaseStorage<ServiceDb> serviceStorage)
        {
            _serviceStorage = serviceStorage;
        }

        public async Task<BaseResponse<List<ServiceViewModel>>> GetAllServices()
        {
            try
            {
                var services = _serviceStorage.GetAll();

                var result = services
                    .Select(x => new ServiceViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        // Маппим новые поля
                        Price = x.Price,
                        Category = x.Category,
                        IsExpress = x.IsExpress
                    })
                    .ToList();

                return new BaseResponse<List<ServiceViewModel>>()
                {
                    Data = result,
                    // ИСПРАВЛЕНО: StatusCode.Ok вместо StatusCode.OK
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ServiceViewModel>>()
                {
                    Description = $"[GetAllServices] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}