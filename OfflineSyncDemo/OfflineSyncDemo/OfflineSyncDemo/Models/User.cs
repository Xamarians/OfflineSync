using OfflineSyncDemo.Data;
using SQLite;

namespace OfflineSyncDemo.Models
{
    public class User : BaseEntity
    {
        [MaxLength(20)]
        [NotNull]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [Unique]
        [NotNull]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(10)]
        [NotNull]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
    }
}
