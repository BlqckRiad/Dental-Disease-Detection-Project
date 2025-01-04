using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLoginRegisterService.EntityLayer.Concrete
{
	public class Role : BaseEntity
	{
		[Key]
        public int RoleID { get; set; }

		public string? RoleName { get; set; }
    }
}
