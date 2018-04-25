using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Idea : BaseEntity
    {   
        public int IdeaID { get; set; }
        public int UserID {get; set;}
        public User User {get; set;}
        public string UserIdea { get; set; }
        public int liked {get; set;} = 0;
        public List<Like> LikedPeople = new List<Like>();
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}