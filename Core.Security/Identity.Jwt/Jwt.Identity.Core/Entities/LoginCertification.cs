using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt.Identity.Core
{
    public class LoginCertification
    {
        public LoginCertification(int errorTimes)
        {
            this.ErrorTimes = errorTimes;
            this.LastErrorTime = DateTime.Now;
        }
        public int ErrorTimes { get; set; }

        public DateTime LastErrorTime { get; set; }
    }
}
