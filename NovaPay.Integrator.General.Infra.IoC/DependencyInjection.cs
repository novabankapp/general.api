using Common.Libraries.Services.EFCore.Repositories;
using Common.Libraries.Services.Flurl.Services;
using Common.Libraries.Services.Repositories;
using Common.Libraries.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovaPay.Integrator.Common.Context;
using NovaPay.Integrator.Common.Data.DTOs;
using NovaPay.Integrator.Common.Data.Entities;
using NovaPay.Integrator.Common.Services.Services.Merchants;
using NovaPay.Integrator.General.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Infra.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterGeneralServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<IApiRequestService, FlurlApiRequestService>();
            services.AddTransient<IApiService, MockApiService>();
            services.AddTransient<ILocalService, LocalService>();
            services.AddTransient<IRepository<RequestLog>, EFRepository<RequestLog, NovaIntegratorContext>>();
            services.AddTransient<IService<RequestLog, RequestLogDto>, Service<RequestLog, RequestLogDto>>();
            services.AddTransient<IRepository<Merchant>, EFRepository<Merchant, NovaIntegratorContext>>();

            services.AddTransient<IService<CustomerReference, CustomerReferenceDto>, Service<CustomerReference, CustomerReferenceDto>>();
            services.AddTransient<IRepository<CustomerReference>, EFRepository<CustomerReference, NovaIntegratorContext>>();


            services.AddTransient<IService<ProcessedTransaction, ProcessedTransactionDto>, Service<ProcessedTransaction,ProcessedTransactionDto>>();
            services.AddTransient<IRepository<ProcessedTransaction>, EFRepository<ProcessedTransaction, NovaIntegratorContext>>();

            services.AddTransient<IService<Merchant, MerchantDto>, Service<Merchant, MerchantDto>>();
            services.AddDbContext<NovaIntegratorContext>(
                 opts => opts.UseMySQL(Configuration["ConnectionStrings:NovaIntegratorDB"]));

        }
    }
}
