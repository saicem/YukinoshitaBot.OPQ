// <copyright file="IMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using YukinoshitaBot.Data.Event;

namespace YukinoshitaBot.Services
{
    /// <summary>
    /// 消息处理器
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// 群文本消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnGroupTextMsgRecieved(TextMessage msg);

        /// <summary>
        /// 群图片消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnGroupPictureMsgRecieved(PictureMessage msg);

        /// <summary>
        /// 好友文本消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnFriendTextMsgRecieved(TextMessage msg);

        /// <summary>
        /// 好友图片消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnFriendPictureMsgRecieved(PictureMessage msg);

        /// <summary>
        /// 临时会话文本消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnTemporaryTextMsgRecieved(TextMessage msg);

        /// <summary>
        /// 临时会话图片消息处理
        /// </summary>
        /// <param name="msg">消息</param>
        void OnTemporaryPictureMsgRecieved(PictureMessage msg);
    }
}
