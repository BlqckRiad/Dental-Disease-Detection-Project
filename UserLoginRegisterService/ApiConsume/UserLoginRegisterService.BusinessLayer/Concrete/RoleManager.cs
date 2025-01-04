using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserLoginRegisterService.BusinessLayer.Abstract;
using UserLoginRegisterService.DataAccessLayer.Abstract;
using UserLoginRegisterService.EntityLayer.Concrete;

namespace UserLoginRegisterService.BusinessLayer.Concrete
{
	public class RoleManager : IRoleService
	{
		private readonly IRoleDal _roleDal;

		public RoleManager(IRoleDal roleDal)
		{
			_roleDal = roleDal;
		}
		public void TAdd(Role entity)
		{
			_roleDal.Add(entity);
		}

		public void TDelete(Role id)
		{
			_roleDal.Delete(id);
		}

		public Role TGetByid(int id)
		{
			return _roleDal.GetByid(id);
		}

		public List<Role> TGetList()
		{
			return _roleDal.GetList();
		}

		public void TUpdate(Role entity)
		{
			_roleDal.Update(entity);
		}
	
	}
}
