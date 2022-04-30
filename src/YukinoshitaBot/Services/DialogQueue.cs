using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YukinoshitaBot.Services
{
    public class DialogQueue : BackgroundService
    {
        private readonly MessageHandler handler;

        public DialogQueue(MessageHandler handler)
        {
            this.handler = handler;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(
                async () =>
                {
                    foreach (var dialog in handler.dialogQueue.GetConsumingEnumerable(stoppingToken))
                    {
                        var resAction = await dialog.HandleAsync();
                    }
                }, stoppingToken);
        }
    }
}
