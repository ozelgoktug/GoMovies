using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string commentText { get; set; }
        public string commentDate { get; set; }
        public int UserId { get; set; }
    }
}
