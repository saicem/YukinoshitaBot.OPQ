// <copyright file="ControllerCollection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;

namespace YukinoshitaBot.Services
{
    /// <summary>
    /// 控制器容器
    /// </summary>
    public class ControllerCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerCollection"/> class.
        /// </summary>
        public ControllerCollection(IServiceProvider serviceProvider, Type[] controllerTypes)
        {
            this.Scope = serviceProvider.CreateScope();
            resolvedControllerDict = (from c in controllerTypes
                                      select new YukinoshitaControllerInfo(c))
                                      .ToDictionary(x => x.TriggerWord, x => x);
        }

        // todo resolvedControllers注入时段

        /// <summary>
        /// 已解析的控制器
        /// </summary>
        private readonly Dictionary<string, YukinoshitaControllerInfo> resolvedControllerDict = null!;
        private IServiceScope Scope { get; set; }

        /// <summary>
        /// 添加一些控制器
        /// </summary>
        /// <param name="controllerTypes">控制器类型列表</param>
        //public ControllerCollection AddControllers(Type[] controllerTypes)
        //{

        //    var resolvedControllerDict = (from c in controllerTypes
        //                select new YukinoshitaControllerInfo(c)).ToDictionary(x => x.TriggerWord, x => x);
                

        
        //    var ci =  new List<YukinoshitaControllerInfo>();
        //    foreach (var controllerType in controllerTypes)
        //    {
        //        ci.Add(new YukinoshitaControllerInfo(controllerType));
        //    }
        //    //(from controllerType in controllerTypes
        //    //                       select new YukinoshitaControllerInfo(controllerType));
        //    Console.Write("ok");
        //    return this;
        //}

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="triggerWord">触发控制器的关键词</param>
        /// <param name="controllerObj">控制器实体</param>
        /// <param name="controllerInfo">控制器信息</param>
        /// <returns></returns>
        public bool TryGetController(
            string triggerWord,
            [NotNullWhen(true)] out YukinoshitaControllerBase? controllerObj,
            [NotNullWhen(true)] out YukinoshitaControllerInfo controllerInfo)
        {
            if (resolvedControllerDict.TryGetValue(triggerWord, out controllerInfo))
            {
                controllerObj = GetController(controllerInfo.ControllerType);
                return true;
            }
            controllerObj = null;
            return false;
        }

        private YukinoshitaControllerBase GetController(Type controllerType)
        {
            if (this.Scope.ServiceProvider.GetService(controllerType) is not YukinoshitaControllerBase controllerObj)
            {
                throw new InvalidOperationException("controller is not resolved.");
            }
            return controllerObj;
        }
    }
}
