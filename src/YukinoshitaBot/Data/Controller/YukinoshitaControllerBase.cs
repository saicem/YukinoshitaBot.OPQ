using System.Collections.Generic;
using System.Linq;
using YukinoshitaBot.Data.Context;
using YukinoshitaBot.Data.Event;

namespace YukinoshitaBot.Data.Controller
{
    /// <summary>
    /// 机器人消息类
    /// </summary>
    public abstract class YukinoshitaControllerBase
    {
        /// <summary>
        /// 接收到的消息
        /// </summary>
        public Message Message { get; set; } = null!;

        /// <summary>
        /// 参数验证中的错误信息
        /// </summary>
        public List<string> ParamErrors { get; set; } = new();

        /// <summary>
        /// 参数验证是否成功
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// 当参数验证失败时调用，重写以自定义参数验证失败时的处理逻辑。
        /// </summary>
        public virtual void OnValidationError()
        {
            if (ParamErrors.Any())
            {
                Message.ReplyText(string.Join('\n', ParamErrors));
            }
        }

        /// <summary>
        /// 对话继续进行，且以指定的方法名作为下一个处理方法。
        /// </summary>
        /// <param name="message"><see cref="BotAction.Message"/></param>
        /// <param name="nextStep"><see cref="BotAction.NextStep"/></param>
        public static BotAction Continue(string message, string nextStep)
        {
            return new BotAction
            {
                Action = DialogAction.Continue,
                Message = message,
                NextStep = nextStep,
            };
        }

        /// <summary>
        /// Terminate the dialog.
        /// </summary>
        /// <param name="message"><see cref="BotAction.Message"/></param>
        /// <returns></returns>
        public static BotAction Terminate(string message)
        {
            return new BotAction
            {
                Action = DialogAction.Terminate,
                Message = message
            };
        }
    }
}
