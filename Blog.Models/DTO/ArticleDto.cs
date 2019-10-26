﻿using System;

namespace Blog.Models.DTO
{
   public class ArticleDto : BaseDto<int>
    {
        public string  Author  { get; set; }
        public string  Title  { get; set; }
        public string  Subtitle  { get; set; }
        public string  ImageUrl  { get; set; }
        public string  Body  { get; set; }
        public DateTime  CreationDate  { get; set; }
    }
}
