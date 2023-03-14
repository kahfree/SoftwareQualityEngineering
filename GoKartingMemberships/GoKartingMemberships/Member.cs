using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoKartingMemberships
{
    class Member
    {
        private string memberType;
        public string MemberType
        {
            get { return memberType; }
        }
        private DateTime signUpDate;
        private DateTime renewalDate;
        private enum Tier { Gold, Silver, Bronze};
        public Member()
        {
            signUpDate = DateTime.Now;
        }

        public bool setRenewalDate(string tier)
        {
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
            return true;
        }

        public bool sendRenewalDiscount()
        {
            int discountPercent = 30;

            if (memberType == Tier.Gold.ToString())
                discountPercent = 24;
            else if (memberType == Tier.Silver.ToString())
                discountPercent = 12;
            else if (memberType == Tier.Bronze.ToString())
                discountPercent = 6;
            try
            {
                //Email sender code goes here
                return true;
            }
            catch(Exception e)
            {
                System.IO.File.WriteAllText(@"c:\Error.txt", e.ToString());// write to event viewer
                return false;
            }
        }

        public void AddToDB()
        {
            try
            {
                // Will call a DAL API here later.
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());// write to event viewer
            }
        }
    }
}
