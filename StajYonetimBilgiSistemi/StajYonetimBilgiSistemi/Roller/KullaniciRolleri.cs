using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace StajYonetimBilgiSistemi.Roller
{

    public class KullaniciRolleri : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        SBYSEntities14 db = new SBYSEntities14();
        public override string[] GetRolesForUser(string username)
        {
            //var kullanici = db.Kullanicilar.FirstOrDefault(x => x.Email == username);
            //return new string[] { kullanici.Rol };

            List<Kullanicilar> kullaniciRolleri = db.Kullanicilar.Where(x => (x.Email == username) || (x.KullaniciAdi == username)).ToList();

            string[] roller = new string[kullaniciRolleri.Count];
            if (kullaniciRolleri.Count > 0)
            {
                for (int i = 0; i < roller.Length; i++)
                {
                    foreach (var item in kullaniciRolleri)
                    {

                        roller[i] = item.Rol.Trim();
                        kullaniciRolleri.Remove(item);
                        break;

                    }

                }
                return roller;
            }
            return new string[] { " " };
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}