using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoKartingMemberships
{
    public class MemberDB
    {
        public MemberDB(FileLogger fileLogger) 
        {
            _logger = fileLogger;
        }
        private FileLogger _logger;
        public void AddToDB(Member member)
        {
            try
            {
                //Add member
            }
            catch (Exception ex)
            {
                _logger.Log("Error adding user to database");
            }
        }
    }
}
