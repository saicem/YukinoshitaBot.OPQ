namespace YukinoshitaBot.Data.Attributes
{
    public enum MatchSource
    {
        /// <summary>
        /// 好友文本消息
        /// </summary>
        FrientText = 1,

        /// <summary>
        /// 群组文本消息
        /// </summary>
        GroupText = 2,

        /// <summary>
        /// 临时会话文本消息
        /// </summary>
        TempText = 4,

        /// <summary>
        /// 好友图片消息
        /// </summary>
        FriendPic = 8,

        /// <summary>
        /// 群组图片消息
        /// </summary>
        GroupPic = 16,

        /// <summary>
        /// 临时会话图片消息
        /// </summary>
        TempPic = 32,

        /// <summary>
        /// 好友的任意消息
        /// </summary>
        FriendAny = FrientText | FriendPic,

        /// <summary>
        /// 群组的任意消息
        /// </summary>
        GroupAny = GroupText | GroupPic,

        /// <summary>
        /// 临时会话的任意消息
        /// </summary>
        TempAny = TempText | TempPic,

        /// <summary>
        /// 任意文本消息
        /// </summary>
        AnyText = FrientText | GroupText | TempText,

        /// <summary>
        /// 任意图片消息
        /// </summary>
        AnyPic = FriendPic | GroupPic | TempPic,
    }
}
