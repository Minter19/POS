using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.User
{
    public class LoginRequest
    {
        public required string Username {  get; set; }
        public required string Password { get; set; }
    }
}
