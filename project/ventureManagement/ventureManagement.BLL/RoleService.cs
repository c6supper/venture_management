using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ventureManagement.DAL;
using ventureManagement.IBLL;
using ventureManagement.models;

namespace ventureManagement.BLL
{
    public class RoleService : BaseService<Role>, InterfaceRoleService
    {
        public RoleService() : base(RepositoryFactory.RoleRepository)
        {
            //default 
            try
            {
                if(Find("administrator") != null) return;

                var adminRole = new Role
                {
                    Name = "administrator",
                    Password = Common.Utility.DesEncrypt("master"),
                    Status = 0,
                    Email = "xxx@163.com",
                    DisplayName = "master",
                    Mobile = "11111111",
                    RegistrationTime = DateTime.Now
                };
                Add(user);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
        }

        public Role Find(string role) { return CurrentRepository.Find(u => u.Name == role); }
    }
}
