using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Cmd.Domain.Entities
{

    [Table("Comment", Schema = "dbo")]
    public class CommentEntity
    {
        public Guid CommentId { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public bool Edit { get; set; }
        public Guid PostId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}
