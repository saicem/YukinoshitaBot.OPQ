// <copyright file="YukinoshitaControllerAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Text;
using System.Text.RegularExpressions;
using YukinoshitaBot.Extensions;

namespace YukinoshitaBot.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class YukinoshitaHandlerAttribute : Attribute
    {
        /// <summary>
        /// 属性辨识符
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 匹配的指令
        /// </summary>
        public string Command { get; set; } = string.Empty;

        public int MatchSource { get; set; }

        public YukinoshitaHandlerAttribute(string Id, string Command)
        {
            this.Id = Id;
            this.Command = Command;
        }

        internal Regex CompileCommand()
        {
            if (string.IsNullOrWhiteSpace(this.Command))
            {
                throw new ArgumentException("Command can't be Empty");
            }
            var sb = new StringBuilder();
            var LeftBracePointer = -1;
            var RightBracePointer = 0;
            while (RightBracePointer < this.Command.Length && this.Command[RightBracePointer] != '{')
            {
                RightBracePointer++;
            }
            sb.Append(Regex.Escape(Command[..RightBracePointer]));
            while (RightBracePointer < this.Command.Length)
            {
                if (this.Command[RightBracePointer] == '{')
                {
                    if (LeftBracePointer != -1)
                    {
                        throw new ArgumentException($"Found another '{{' in command {Command} with a '{{' not paired.");
                    }
                    LeftBracePointer = RightBracePointer;
                }
                else if (this.Command[RightBracePointer] == '}')
                {
                    if (LeftBracePointer == -1)
                    {
                        throw new ArgumentException($"Found '}}' in command {Command}, but can't found the corresponding '{{'.");
                    }
                    // todo 如何不复制 直接使用对应的地址
                    sb.Append(ParseCommandUnit(Command[(LeftBracePointer + 1)..RightBracePointer]));
                    LeftBracePointer = -1;
                }
                RightBracePointer++;
            }
            return new Regex(sb.ToString());
        }

        private static string ParseCommandUnit(in string input)
        {
            // TODO 更丰富的类型 append
            var match = Regex.Match(input, @"^(?<key>[a-zA-Z0-9@_]+)(?<append>:[a-z]+)?(?<option>\?)?$");
            if (!match.Success)
            {
                throw new ArgumentException($"\"{input}\" is not a valid command unit.");
            }

            var key = match.Groups["key"].Value;
            if (!key.IsValidIdentifier())
            {
                throw new ArgumentException($"\"{key}\" is not valid identifier.");
            }

            var append = match.Groups["append"]?.Value;
            if (append != null)
            {
                // TODO 此处依照 append模式进行匹配
                // TODO 还没写
            }

            // TODO option 必须放在最后
            var option = match.Groups["option"];
            if (option != null)
            {
                return @$"(?:\s+(?<{key}>\S+))?";
            }
            else
            {
                return @$"\s+(?<{key}>\S+)";
            }
        }
    }
}
