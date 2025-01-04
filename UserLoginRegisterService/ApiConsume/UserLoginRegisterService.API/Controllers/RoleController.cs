using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserLoginRegisterService.BusinessLayer.Abstract;
using UserLoginRegisterService.DtoLayer.Dtos.Role;
using UserLoginRegisterService.EntityLayer.Concrete;

namespace UserLoginRegisterService.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class RoleController : ControllerBase
	{
		public IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

		[HttpGet]
		public IActionResult GetRoles()
		{
			var roles = _roleService.TGetList().Where(x=> x.IsDeleted == false);
			return Ok(roles);
		}
		[HttpPost]
		public IActionResult AddRole(AddRoleDto model)
		{
			Role data = new Role();

			data.RoleName = model.RoleName;
			data.CreatedDate = DateTime.Now;
			data.CreatedUserID = model.CreatedUserID;

			_roleService.TAdd(data);
			return Ok(data);
		}
		[HttpPost]
		public IActionResult UpdateRole (UpdateRoleDto model)
		{
			var role = _roleService.TGetByid(model.RoleId);
			role.RoleName = model.RoleName;
			role.UpdatedDate = DateTime.Now;
			role.UpdatedUserID = model.UpdatedUserID;
			_roleService.TUpdate(role);
			return Ok(role);
		}
    }
}
