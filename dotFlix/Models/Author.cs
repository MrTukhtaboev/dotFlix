using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotFlix.Models
{
    [Table("authors")]
    public class Author
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [MaxLength(50), Column("firstname")]
        public string FirstName { get; set; }

        [MaxLength(50), Column("lastname")]
        public string LastName { get; set; }
    }
}