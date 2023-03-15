using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoKartingMemberships
{
    public class EmailSender
    {
        public EmailSender() { }
        public virtual bool SendEmail(String body, String subject, String message)
        {
            return true;
        }
    }
}
