using System;
using System.Collections.Generic;
using System.Text;

namespace Teng.Infrastructure.Users.dtos
{
    public class LoginInputDto
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}