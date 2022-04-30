using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Context;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Extensions;

namespace YukinoshitaBot.Data.Attributes
{
    public class YukinoshitaHandlerInfo
    {
        private readonly MethodInfo methodInfo;
        private readonly ParameterInfo[] methodParameters;

        private readonly YukinoshitaHandlerAttribute HandlerAttribute;
        public string HandlerId => HandlerAttribute.Id;

        public bool IsAsync { get; }
        private readonly Regex matchRegex;

        public YukinoshitaHandlerInfo(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
            methodParameters = methodInfo.GetParameters();
            this.HandlerAttribute = methodInfo.GetCustomAttribute<YukinoshitaHandlerAttribute>()
                ?? throw new Exception("使用了不为 YukinoshitaHandler 的方法");
            matchRegex = HandlerAttribute.CompileCommand();
            IsAsync = methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
        }

        public Task<BotAction> InvokeAsync(YukinoshitaControllerBase controller, in object[] @params)
        {
            if (IsAsync)
            {
                return methodInfo.Invoke(controller, @params) as Task<BotAction>
                       ?? throw new Exception($"方法 {methodInfo.Name} 没有返回 Task<BotAction> 类型");
            }
            else
            {
                return Task.FromResult(methodInfo.Invoke(controller, @params) as BotAction
                       ?? throw new Exception($"方法 {methodInfo.Name} 没有返回 BotAction 类型"));
            }
        }

        public bool TryMatch(in string input, out Dictionary<string, string> matchPairs)
        {
            return matchRegex.TryGetMatchPairs(input, out matchPairs);
        }

        public bool TryValidParams(in Dictionary<string, string> matchPairs, out object[] @params, out List<string> paramErrors)
        {
            @params = new object[methodParameters.Length];
            paramErrors = new List<string>();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var name = methodParameters[i].Name ?? throw new Exception("Name can't be null.");
                if (matchPairs.TryGetValue(name, out var value))
                {
                    if (TryValidParam(value, methodParameters[i], out var errorInfo))
                    {
                        @params[i] = Convert.ChangeType(value, methodParameters[i].ParameterType);
                        continue;
                    }
                    else
                    {
                        if (errorInfo != null)
                        {
                            // TODO as enumerable
                            paramErrors.Add(errorInfo);
                        }
                    }
                }
                else
                {
                    @params[i] = methodParameters[i].DefaultValue
                        ?? throw new ArgumentException($"can't get the value of key:{name} from the command, and the parameter doesn't have a default value, please check your command.");
                }
            }

            return false;
        }

        private static bool TryValidParam(string value, ParameterInfo info, out string? errorInfo)
        {
            var attrs = info.GetCustomAttributes<ValidationAttribute>();
            foreach (var attr in attrs)
            {
                if (!attr.IsValid(value))
                {
                    errorInfo = attr.ErrorMessage;
                    return false;
                }
            }
            errorInfo = null;
            return true;
        }
    }
}
