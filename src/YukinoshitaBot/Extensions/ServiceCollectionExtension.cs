// <copyright file="ServiceCollectionExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Services;

namespace YukinoshitaBot.Extensions
{
    /// <summary>
    /// 依赖注入拓展
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加YukinoshitaBot服务，使用自定义消息处理器
        /// </summary>
        /// <typeparam name="MessageHandlerType">要使用的消息处理器类型</typeparam>
        /// <param name="services">服务容器</param>
        /// <returns>链式调用服务容器</returns>
        public static IServiceCollection AddYukinoshitaBot<MessageHandlerType>(this IServiceCollection services)
            where MessageHandlerType : class, IMessageHandler
        {
            services.AddSingleton<OpqApiHandler>();
            services.AddHostedService<MessageQueueScanner>();
            services.AddScoped<IMessageHandler, MessageHandlerType>();
            services.AddHostedService<YukinoshitaWorker>();

            return services;
        }

        /// <summary>
        /// 添加YukinoshitaBot服务，使用YukinoshitaController处理消息
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns>链式调用服务容器</returns>
        public static IServiceCollection AddYukinoshitaBot(this IServiceCollection services)
        {
            services.AddSingleton<OpqApiHandler>();
            services.AddMemoryCache();

            // 扫描当前程序集，添加所有带有 YukinoshitaControllerAttribute 并且继承于 YukinoshitaControllerBase 的服务作为控制器
            var ass = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var controllerTypes = (from type in ass.GetTypes()
                                   where type.GetCustomAttribute<YukinoshitaControllerAttribute>() is not null
                                   where type.BaseType == typeof(YukinoshitaControllerBase)
                                   select type).ToArray();

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            // ControllerCollection维护所有Controller的类型信息
            services.AddSingleton(services => new ControllerCollection(services, controllerTypes));

            services.AddSingleton<IMessageHandler, MessageHandler>();

            services.AddHostedService<MessageQueueScanner>();
            services.AddHostedService<YukinoshitaWorker>();

            return services;
        }
    }
}

