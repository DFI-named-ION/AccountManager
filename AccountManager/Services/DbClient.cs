using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AccountManager.Core;
using AccountManager.MVVM.Models;

namespace AccountManager.Services
{
    public static class DbClient
    {
        static private Data _context = new Data();

        static public bool CheckConnection()
        {
            try
            {
                return new Ping().Send(@"www.google.com").Status == IPStatus.Success;
            }
            catch(Exception)
            { return false; }
        }

        static public User LogInUser(string login, string password)
        {
            try
            {
                if (!CheckConnection())
                    return null;

                string crypt = DataCryptography.MD5Encrypt(password);
                return _context.Users.Where(x => x.Login == login && x.Password == crypt).FirstOrDefault();
            }
            catch (Exception)
            { return null; }
        }

        static public User SignInUser(string login, string password)
        {
            try
            {
                if (!CheckConnection())
                    return null;

                User user = null;
                if (!UserExist(login, password))
                {
                    string crypt = DataCryptography.MD5Encrypt(password);
                    user = CreateUser(login, crypt);
                }
                return user;
            }
            catch (Exception)
            { return null; }
        }

        static private bool UserExist(string login, string password)
        {
            try
            {
                if (!CheckConnection())
                    return true;
                string crypt = DataCryptography.MD5Encrypt(password);
                return _context.Users.Where(x => x.Login == login && x.Password == crypt).FirstOrDefault() != null;
            }
            catch (Exception)
            { return true; }
        }

        static public void RemoveGroup(Group g)
        {
            try
            {
                if (!CheckConnection())
                    return;

                if (g != null)
                {
                    _context.Groups.Remove(g);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            { return; }
        }

        static public void CommitData()
        {
            try
            {
                if (!CheckConnection())
                    return;
                _context.SaveChanges();
            }
            catch (Exception)
            { return; }
        }

        static public List<Group> GetGroups(User user)
        {
            try
            {
                if (!CheckConnection())
                    return null;

                return _context.Groups.Where(x => x.UserID == user.ID).ToList();
            }
            catch (Exception)
            { return null; }
        }

        static private User CreateUser(string login, string password)
        {
            try
            {
                if (!CheckConnection())
                    return null;

                var user = _context.Users.Create();

                user.Login = login;
                user.Password = password;
                _context.Users.Add(user);
                _context.SaveChanges();

                return _context.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
            }
            catch (Exception)
            { return null; }
        }

        static public Group CreateGroup(User user, string title, string desc, string path)
        {
            try
            {
                if (!CheckConnection())
                    return null;
                var group = _context.Groups.Create();

                group.User = user;
                group.UserID = user.ID;
                group.Title = DataCryptography.AESEncrypt(title);
                group.Description = DataCryptography.AESEncrypt(desc);
                group.ImagePath = DataCryptography.AESEncrypt(path);
                _context.Groups.Add(group);
                _context.SaveChanges();

                return _context.Groups.Where(x => x.ID == group.ID).FirstOrDefault();
            }
            catch (Exception) 
            { return null; }
        }

        static public List<Account> GetAccounts(User user, Group group)
        {
            try
            {
                if(!CheckConnection())
                    return null;

                return _context.Accounts.Where(x => x.UserID == user.ID && x.GroupID == group.ID).ToList();
            }
            catch(Exception)
            { return null; }
        }

        static public void RemoveAccount(Account a)
        {
            try
            {
                if (!CheckConnection())
                    return;

                if(a != null)
                {
                    _context.Accounts.Remove(a);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            { return; }
        }

        static public Account CreateAccount(User user, Group group, string title, string login, string password, string email)
        {
            try
            {
                if(!CheckConnection())
                    return null;

                var account = _context.Accounts.Create();

                account.GroupID = group.ID;
                account.User = user;
                account.Title = DataCryptography.AESEncrypt(title);
                account.Login = DataCryptography.AESEncrypt(login);
                account.Password = DataCryptography.AESEncrypt(password);
                account.Email = DataCryptography.AESEncrypt(email);
                account.UserID = user.ID;
                _context.Accounts.Add(account);
                _context.SaveChanges();
                
                return account;
            }
            catch (Exception)
            { return null; }
        }
    }
}
