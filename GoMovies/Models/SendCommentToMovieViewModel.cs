using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class SendCommentToMovieViewModel
    {
        public int MoveiId { get; set; }
        public string CommentTittle { get; set; }
        public string CommentText { get; set; }
        public int  CommentUserId { get; set; }
    }
}
