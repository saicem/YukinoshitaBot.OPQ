// <copyright file="YukinoshitaController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Context;
using YukinoshitaBot.Data.Event;

namespace YukinoshitaBot.Services
{
    /// <summary>
    /// 实现控制器
    /// </summary>
    public class MessageHandler : IMessageHandler
    {
        private readonly ControllerCollection controllerCollection;

        internal BlockingCollection<Dialog> dialogQueue = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler"/> class.
        /// </summary>
        /// <param name="controllerCollection">控制器容器</param>
        public MessageHandler(
            ControllerCollection controllerCollection)
        {
            this.controllerCollection = controllerCollection;
        }

        /// <inheritdoc/>
        public void OnFriendPictureMsgRecieved(PictureMessage msg)
        {
            OnPictureMsgRecieved(msg);
        }

        /// <inheritdoc/>
        public void OnFriendTextMsgRecieved(TextMessage msg)
        {
            OnTextMsgRecieved(msg);
        }

        /// <inheritdoc/>
        public void OnGroupPictureMsgRecieved(PictureMessage msg)
        {
            OnPictureMsgRecieved(msg);
        }

        /// <inheritdoc/>
        public void OnGroupTextMsgRecieved(TextMessage msg)
        {
            OnTextMsgRecieved(msg);
        }

        /// <inheritdoc/>
        public void OnTemporaryPictureMsgRecieved(PictureMessage msg)
        {
            OnPictureMsgRecieved(msg);
        }

        /// <inheritdoc/>
        public void OnTemporaryTextMsgRecieved(TextMessage msg)
        {
            OnTextMsgRecieved(msg);
        }

        /// <summary>
        /// 文本消息处理
        /// </summary>
        /// <param name="msg">文本消息</param>
        /// <exception cref="NullReferenceException"></exception>
        public void OnTextMsgRecieved(TextMessage msg)
        {
            if (HandlePersonDialog(msg))
            {
                return;
            }
            if (HandleGroupDialog(msg))
            {
                return;
            }
            HandleNewDialog(msg);
        }

        private bool HandleNewDialog(in TextMessage msg)
        {
            // todo 控制传入文本长度 忽略超长文本
            string triggerWord = msg.Content.Split()[0];
            if (controllerCollection.TryGetController(triggerWord, out var controllerObj, out var controllerInfo))
            {
                dialogQueue.Add(new Dialog(msg.SenderInfo.FromQQ, controllerObj, controllerInfo.EntryHandlerId, msg));
            }
            return false;
        }

        private bool HandleGroupDialog(in TextMessage msg)
        {
            throw new NotImplementedException();
        }

        private bool HandlePersonDialog(in TextMessage msg)
        {
            throw new NotImplementedException();
        }

        public static void OnPictureMsgRecieved(PictureMessage msg)
        {
        }
    }
}
