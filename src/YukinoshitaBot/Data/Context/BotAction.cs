namespace YukinoshitaBot.Data.Context
{
    public record BotAction
    {
        /// <summary>
        /// 对话的处理状态
        /// </summary>
        public DialogAction Action { get; set; } = DialogAction.Terminate;

        /// <summary>
        /// 返回给消息源的消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 当 ActionType 为 Continue 时的下一个处理方法标识
        /// </summary>
        public string NextStep { get; set; } = string.Empty;
    }

    public enum DialogAction
    {
        /// <summary>
        /// 未能匹配当前对话
        /// </summary>
        NotMatch,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        ValidError,

        /// <summary>
        /// 继续进行当前对话
        /// </summary>
        Continue,

        /// <summary>
        /// 结束当前对话
        /// </summary>
        Terminate,
    }
}
