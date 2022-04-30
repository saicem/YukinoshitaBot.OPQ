using System;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;

namespace YukinoshitaBot.Data.Context
{
    public class Dialog
    {
        public Dialog(long id, YukinoshitaControllerBase controller, string handlerId, TextMessage textMessage)
        {
            Id = id;
            this.controller = controller;
            HandlerId = handlerId;
            this.TextMessage = textMessage;
            IsReady = true;
        }

        /// <summary>
        /// 对话 ID, 推荐使用 QQ 号或者群号辨别。
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// 对话是否收到了消息
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// 用于该对话的控制器
        /// </summary>
        protected readonly YukinoshitaControllerBase controller;

        /// <summary>
        /// 下一个处理用的方法
        /// </summary>
        public YukinoshitaHandlerInfo Method { get; set; } = null!;

        /// <summary>
        /// 下一个处理方法的 ID
        /// </summary>
        public string HandlerId { get; set; } = null!;

        /// <summary>
        /// 对话的开始时间
        /// </summary>
        public DateTime StartTime { get; private init; } = DateTime.Now;

        /// <summary>
        /// 对话等待的时长
        /// </summary>
        public TimeSpan WaitTime { get; init; } = Option.Option.DialogWaitTime;

        /// <summary>
        /// 对话结束的时间
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// 对话消息源的文本消息
        /// </summary>
        public TextMessage TextMessage { get; set; }

        /// <summary>
        /// 刷新对话到期时间
        /// </summary>
        public void RefreshEndTime()
        {
            EndTime = DateTime.Now + WaitTime;
        }

        public void LoadMessage(TextMessage msg)
        {
            TextMessage = msg;
            IsReady = true;
        }

        /// <summary>
        /// 尝试处理文本
        /// </summary>
        public async Task<BotAction> HandleAsync()
        {
            if (!Method.TryMatch(TextMessage.Content, out var matchPairs))
            {
                return new BotAction { Action = DialogAction.NotMatch };
            }
            if (!Method.TryValidParams(matchPairs, out var @params, out var paramErrors))
            {
                return new BotAction { Action = DialogAction.ValidError, Message = string.Join('\n', paramErrors) };
            }
            // todo 定制化的选择 对话结束的时间
            IsReady = false;
            if (Method.IsAsync)
            {
                return await Method.InvokeAsync(controller, @params).ConfigureAwait(false);
            }
            else
            {
                // TODO 异步和非异步有没有区别 可不可以把非异步的转化为 task
                return await Method.InvokeAsync(controller, @params);
            }
        }
    }
}
