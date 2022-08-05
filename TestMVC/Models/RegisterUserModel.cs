using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models
{
    public class RegisterUserModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
