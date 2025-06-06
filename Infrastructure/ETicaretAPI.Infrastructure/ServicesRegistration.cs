﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Interfaces;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Infrastructure
{
    public static class ServicesRegistration 
    {
        public static void AddInfrastructureServices(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddScoped<IStorageService, StorageService>();
            servicesCollection.AddScoped<IQueryPagingService, QueryPagingService>();
        }

        public static void AddStorage<T>(this IServiceCollection servicesCollection) where T : Storage, IStorage
        {
            servicesCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection servicesCollection, StorageType storageType) 
        {
            switch (storageType) { 
            
                case StorageType.Local:
                    servicesCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    servicesCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    servicesCollection.AddScoped<IStorage, LocalStorage>();
                    break;

            }

        }
    }
}
