using System;

namespace YukinoshitaBot.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class YukinoshitaControllerAttribute : Attribute
    {
        public YukinoshitaControllerAttribute(string triggerWord, string beginHandleId)
        {
            TriggerWord = triggerWord;
            BeginHandleId = beginHandleId;
        }

        /// <summary>
        /// 触发会话的关键词
        /// </summary>
        public string TriggerWord { get; set; } = null!;

        /// <summary>
        /// 当会话触发后启动的处理方法
        /// </summary>
        public string BeginHandleId { get; set; } = null!;
    }
}
