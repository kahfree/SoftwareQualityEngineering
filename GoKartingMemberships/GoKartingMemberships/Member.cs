using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoKartingMemberships
{
    public class Member
    {
        private string memberType;
        public string MemberType
        {
            get { return memberType; }
        }
        private DateTime signUpDate;
        private DateTime renewalDate;
        public DateTime SignUpDate { get { return signUpDate; } }
        public DateTime RenewalDate { get { return renewalDate; } }
        private enum Tier { Gold, Silver, Bronze};

        private EmailSender emailSender;
        private FileLogger fileLogger;
        public Member(EmailSender emailSender, FileLogger fileLogger)
        {
            signUpDate = DateTime.Now;
            this.emailSender = emailSender;
            this.fileLogger = fileLogger;
        }

        public bool setRenewalDate(string tier)
        {
            renewalDate = DateTime.Now;
            int membershipLength = 30;
            for (Tier memPri = Tier.Gold; memPri <= Tier.Bronze;
                 memPri++)
            {
                if (tier == memPri.ToString())
                {
                    if (memPri == Tier.Gold)
                        membershipLength = 24;
                    else if (memPri == Tier.Silver)
                        membershipLength = 12;
                    else if (memPri == Tier.Bronze)
                        membershipLength = 6;
                    memberType = memPri.ToString();
                    renewalDate = DateTime.Now.AddMonths(membershipLength);
                }
            }
            return renewalDate != DateTime.Now;
        }

        public bool sendRenewalDiscount()
        {
            try
            {
                emailSender.SendEmail($"You got {calculateDiscountPercentage()}% off", "Renewal Discount Price", "You got mail!");
                return true;
            }
            catch(Exception e)
            {
                fileLogger.Log($"Error sending email for renewal discount: {e}");
                return false;
            }
        }

        public int calculateDiscountPercentage()
        {
            if (memberType == Tier.Gold.ToString())
                return 24;
            else if (memberType == Tier.Silver.ToString())
                return 12;
            else if (memberType == Tier.Bronze.ToString())
                return 6;
            else
                return 0;
        }
    }
}
