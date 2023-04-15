using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
    }
}