using System.ComponentModel.DataAnnotations;

namespace communication.API.Dtos
{
    public class UserForRegisterInputDto
    {
        [Required]
        public string Username { get; set; }
        [Required, StringLength(maximumLength: 8, MinimumLength = 4, ErrorMessage = "يجب الاتقل كلمة المرور عن 4 حروف ولا تزيد عن 8")]
        public string Password { get; set; }




    }
}