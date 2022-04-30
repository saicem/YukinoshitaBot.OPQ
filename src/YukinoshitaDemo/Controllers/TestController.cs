using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Context;
using YukinoshitaBot.Data.Controller;

namespace YukinoshitaDemo.Controllers
{
    [YukinoshitaController("测试", "1")]
    public class TestController : YukinoshitaControllerBase
    {   
        public TestController()
        {

        }

        [YukinoshitaHandler("1", "测试{p1}{p2?}")]
        public static BotAction TestHandler(string p1, string p2 = "你好")
        {
            return Terminate($"p1: {p1}, p2: {p2}");
        }
    }
}
