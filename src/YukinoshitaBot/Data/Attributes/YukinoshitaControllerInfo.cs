// <copyright file="YukinoshitaControllerInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YukinoshitaBot.Data.Attributes
{
    /// <summary>
    /// 控制器信息
    /// </summary>
    public class YukinoshitaControllerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YukinoshitaControllerInfo"/> struct.
        /// </summary>
        /// <param name="controllerType">控制器类型</param>
        public YukinoshitaControllerInfo(Type controllerType)
        {
            this.ControllerType = controllerType;
            var attribute = controllerType.GetCustomAttribute<YukinoshitaControllerAttribute>();
            if (attribute is null)
            {
                throw new InvalidCastException($"Type '{controllerType.FullName}' is not a YukinoshitaController.");
            }
            this.ControllerAttribute = attribute;

            var methodInfos = controllerType.GetMethods();

            handlerInfos = (from methodInfo in methodInfos
                            let attr = methodInfo.GetCustomAttribute<YukinoshitaHandlerAttribute>()
                            where attr != null
                            select new YukinoshitaHandlerInfo(methodInfo))
                                 .ToDictionary(info => info.HandlerId, info => info);
        }

        /// <summary>
        /// 控制器类型
        /// </summary>
        public Type ControllerType { get; }

        /// <summary>
        /// 处理方法
        /// </summary>
        private readonly Dictionary<string, YukinoshitaHandlerInfo> handlerInfos;

        /// <summary>
        /// 获取对应 Id 的 handler
        /// </summary>
        /// <param name="handlerId"></param>
        /// <returns></returns>
        public YukinoshitaHandlerInfo GetHandlerInfo(string handlerId)
        {
            return handlerInfos[handlerId];
        }

        /// <summary>
        /// 控制器属性
        /// </summary>
        private readonly YukinoshitaControllerAttribute ControllerAttribute;

        public string TriggerWord => ControllerAttribute.TriggerWord;

        public string EntryHandlerId => ControllerAttribute.BeginHandleId;
    }
}
