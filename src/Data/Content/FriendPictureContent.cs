﻿// <copyright file="FriendPictureContent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace YukinoshitaBot.Data.Content
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 好友图片消息
    /// </summary>
    public class FriendPictureContent
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<PictureInfo>? FriendPic { get; set; }

        /// <summary>
        /// 文件大小，仅当图片为闪照时有效
        /// </summary>
        public int? FileSize { get; set; }

        /// <summary>
        /// 文件URL，仅当图片为闪照时有效
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 是否闪照
        /// </summary>
        [JsonIgnore]
        public bool IsFlashPicture => !string.IsNullOrEmpty(this.Url);
    }
}
