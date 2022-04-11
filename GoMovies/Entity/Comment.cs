using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string commentText { get; set; }
        public string commentDate { get; set; }

        public int MovieId { get; set; }
        public int ApplicationUserId { get; set; }
        public Movie Movie { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
