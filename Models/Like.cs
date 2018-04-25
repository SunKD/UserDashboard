using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Like : BaseEntity
    {
        public int LikeID { get; set; }
        public int likedCount { get; set; } = 0;
        public int UserID { get; set; }
        public User User { get; set; }
        public int IdeaID { get; set; }
        public Idea Idea { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}