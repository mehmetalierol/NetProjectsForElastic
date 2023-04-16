using System.ComponentModel.DataAnnotations;

namespace ApiWithEntityFramework
{
    public class MailLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
    }
}
