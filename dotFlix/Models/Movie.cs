using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sysJson = System.Text.Json.Serialization;
using newJson = Newtonsoft.Json;

namespace dotFlix.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key, Column("id")]
        public long Id { get; set; }
        
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
        [Column("image")]
        public string Image { get; set; }

        [ForeignKey(nameof(Author)), Column("authorid")]
        public long AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
