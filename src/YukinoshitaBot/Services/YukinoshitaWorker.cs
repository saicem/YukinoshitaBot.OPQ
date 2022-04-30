// <copyright file="YukinoshitaWorker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocketIOClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Event;
using YukinoshitaBot.Data.WebSocket;
using YukinoshitaBot.Services;

namespace YukinoshitaBot
{
    /// <summary>
    /// 工作线程
    /// </summary>
    public class YukinoshitaWorker : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly OpqApiHandler opqApi;
        private readonly IMessageHandler msgHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="YukinoshitaWorker"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="configuration">config</param>
        /// <param name="opqApi">opqApi</param>
        /// <param name="msgHandler">message handler</param>
        public YukinoshitaWorker(ILogger<YukinoshitaWorker> logger, IConfiguration configuration, OpqApiHandler opqApi, IMessageHandler msgHandler)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.opqApi = opqApi;
            this.msgHandler = msgHandler;
        }

        /// <inheritdoc/>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var botConfig = this.configuration.GetSection("OpqApiSettings");
            var httpApi = botConfig.GetValue<string>("HttpApi");
            var loginQQ = botConfig.GetValue<long>("LoginQQ");
            var wsApi = botConfig.GetValue<string>("WebSocketApi");

            this.logger.LogInformation("Starting YukinoshitaBot...");
            this.logger.LogInformation("HttpApi: {httpApi}", httpApi);
            this.logger.LogInformation("LoginQQ: {loginQQ}", loginQQ);
            this.logger.LogInformation("WsApi: {wsApi}", wsApi);
            await this.NewClientAsync(wsApi);
        }

        private async Task NewClientAsync(string wsApi)
        {
            var client = new SocketIO(wsApi);
            client.On("OnGroupMsgs", resp =>
            {
                OnGroupMsgs(resp);
            });
            client.On("OnFriendMsgs", resp =>
            {
                OnFriendMsgs(resp);
            });
            client.OnConnected += (sender, e) =>
            {
                this.logger.LogInformation("YukinoshitaBot is now connected.");
            };

            await client.ConnectAsync();
        }

        private void OnFriendMsgs(SocketIOResponse resp)
        {
            this.logger.LogDebug(resp.ToString());
            var respData = resp.GetValue<SocketResponse<FriendMessage>>();

            // 过滤自身消息
            if (respData.CurrentPacket?.Data?.FromUin == respData.CurrentQQ)
            {
                return;
            }

            Message? msg = null;
            try
            {
                msg = Message.Parse(respData.CurrentPacket?.Data);
                msg.OpqApi = this.opqApi;
            }
            catch (Exception e)
            {
                this.logger.LogError("Message parse failed : {error}", e);
                return;
            }

            switch (msg)
            {
                case TextMessage textMsg:
                    this.msgHandler.OnFriendTextMsgRecieved(textMsg);
                    break;
                case PictureMessage picMsg:
                    this.msgHandler.OnFriendPictureMsgRecieved(picMsg);
                    break;
                default:
                    this.logger.LogWarning("Unresolved message object Type {type}", msg.GetType());
                    break;
            }
        }

        private void OnGroupMsgs(SocketIOResponse resp)
        {
            this.logger.LogDebug(resp.ToString());
            // TODO 收到红包的话这个解析好像会寄
            var respData = resp.GetValue<SocketResponse<GroupMessage>>();

            // 过滤自身消息
            if (respData.CurrentPacket?.Data?.FromUserId == respData.CurrentQQ)
            {
                return;
            }

            Message? msg = null;
            try
            {
                msg = Message.Parse(respData.CurrentPacket?.Data);
                msg.OpqApi = this.opqApi;
            }
            catch (Exception e)
            {
                this.logger.LogError("Message parse failed : {error}", e);
                return;
            }

            switch (msg)
            {
                case TextMessage textMsg:
                    this.msgHandler.OnGroupTextMsgRecieved(textMsg);
                    break;
                case PictureMessage picMsg:
                    this.msgHandler.OnGroupPictureMsgRecieved(picMsg);
                    break;
                default:
                    this.logger.LogWarning("Unresolved message object Type {type}", msg.GetType());
                    break;
            }
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
