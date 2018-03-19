using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDao.EF;

namespace UniversityDao.Dao {
    public class AccountDao {
        UniversityDbContext db = new UniversityDbContext();
        public Account GetUserByUserName(string userName) {
            Account account = db.Accounts.SingleOrDefault(x => x.Username.Equals(userName));
            return account;
        }

        public Account GetUserByID(int id) {
            Account account = db.Accounts.Find(id);
            return account;
        }

        public List<Account> GetAllAccount() {
            return db.Accounts.ToList();
        }

        public int LoginCheck(LoginModel account) {
            int status = 0;
            var result = db.Accounts.SingleOrDefault(x => x.Username == account.Username);
            if (result == null) {
                status = 0;
            } else {
                if (result.Status == false) {
                    status = -2;
                } else if (result.Status == true) {
                    if (result.Password == account.Password) {
                        status = 1;
                    } else {
                        status = -1;
                    }
                }
            }
            return status;
        }

        public bool CreateAccount(Account model) {
            try {
                db.Accounts.Add(model);
                db.SaveChanges();
                return true;
            } catch {

            }
            return false;
        }

        public bool EditAccount(Account model) {
            try {
                var item = db.Accounts.Find(model.UserID);
                item.Username = model.Username;
                item.Status = model.Status;
                item.Role = model.Role;
                item.Phone = model.Role;
                item.Password = model.Password;
                item.FullName = model.FullName;
                item.Email = model.Email;
                item.Address = model.Address;
                db.SaveChanges();
                return true;
            } catch {

            }
            return false;
        }

        public bool DeleteAccount(Account model) {
            try {
                db.Accounts.Remove(model);
                db.SaveChanges();
                return true;
            } catch {

            }
            return false;
        }

        public bool UploadImage(int id, String file) {
            try {
                var item = db.Accounts.Find(id);
                item.Image = file;
                db.SaveChanges();
                return true;
            } catch {

            }
            return false;
        }

        public int Registry(Account model)
        {
            Account account = new Account();
            account.Password = model.Password;
            account.Username = model.Username;
            account.Email = model.Email;
            account.FullName = model.FullName;
            account.Address = model.Address;
            account.Phone = model.Phone;
            account.Status = true;
            account.Role = "STU";
            account.EmailConfirmed = false;
            db.Accounts.Add(account);
            db.SaveChanges();
            return account.UserID;
        }

        public bool UpdateEmailConfirmed(int userId)
        {
            try
            {
                Account account = db.Accounts.SingleOrDefault(x => x.UserID == userId);
                account.EmailConfirmed = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
