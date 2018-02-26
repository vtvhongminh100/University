using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDao.EF;

namespace UniversityDao.Dao
{
    public class AccountDao
    {
        UniversityDbContext db = new UniversityDbContext();
        public Account GetUserByUserName(string userName)
        {
            Account account = db.Accounts.SingleOrDefault(x => x.Username.Equals(userName));
            return account;
        }
        public int LoginCheck(AccountModel account)
        {
            int status = 0;
            var result = db.Accounts.SingleOrDefault(x => x.Username == account.Username);
            if (result == null)
            {
                status = 0;
            }
            else
            {
                if (result.Status == false)
                {
                    status = -2;
                }
                else if (result.Status == true)
                {
                    if (result.Password == account.Password)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = -1;
                    }
                }
            }
            return status;
        }
    }
}
