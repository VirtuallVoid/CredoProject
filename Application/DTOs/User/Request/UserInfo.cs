using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.User.Request
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
