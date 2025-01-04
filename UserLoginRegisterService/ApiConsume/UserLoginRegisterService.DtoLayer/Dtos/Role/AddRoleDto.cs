using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLoginRegisterService.DtoLayer.Dtos.Role
{
	public class AddRoleDto
	{
		public string RoleName { get; set;}
		public int CreatedUserID { get; set; }
	}
}
