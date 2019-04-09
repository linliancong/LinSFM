using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinSFMSys
{
    class Role
    {
        private int RoleID;
        private string RoleName;
        public Role(int RoleID, string RoleName) {
            this.RoleID = RoleID;
            this.RoleName = RoleName;
        }

        public void setRoleID(int RoleID){
            this.RoleID = RoleID;
        }

        public void setRoleName(string RoleName)
        {
            this.RoleName = RoleName;
        }

        public int getRoleID()
        {
            return this.RoleID;
        }

        public string getRoleName()
        {
            return this.RoleName;
        }
    }
}
