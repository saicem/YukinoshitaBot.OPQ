using System;

namespace YukinoshitaBot.Data.Option
{
    public static class Option
    {
        public static TimeSpan DialogWaitTime { get; set; } = TimeSpan.FromSeconds(45);

        public static WhenValidError WhenValidError { get; set; } = WhenValidError.Default;

    }

    public enum WhenValidError
    {
        /// <summary>
        /// 不返回任何错误消息
        /// </summary>
        Silent,

        /// <summary>
        /// 返回错误消息
        /// </summary>
        Default,
    }
}
