using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using REC.Inventory.Infrastructure;
using REC.Inventory.Infrastructure.Identity;
using REC.Inventory.Web.Areas.Admin.Models;

namespace REC.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MemberController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemberController(RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
        {
            return View();
        }

		[HttpPost, Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUserRolesJsonData()
		{
			var users = await _userManager.Users.ToListAsync();
			var userRolesData = new List<UserRoleModel>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
                userRolesData.Add(new UserRoleModel
                {
                    UserName = user.UserName,
                    Role = string.Join(", ", roles)
                });
			}

			return Json(userRolesData);
		}


		[Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            var model = new RoleCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        NormalizedName = model.RoleName.ToUpper(),
                        Name = model.RoleName,
                        ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                    });
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role create successfull.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role create failed.",
                        Type = ResponseTypes.Danger
                    });
                }
                
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole()
        {
            var model = new RoleChangeModel();
            LoadValues(model);
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(RoleChangeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role change successfull.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role change failed.",
                        Type = ResponseTypes.Danger
                    });
                }
                
            }
            LoadValues(model);
            return View(model);
        }

        private void LoadValues(RoleChangeModel model)
        {
            var users = from c in _userManager.Users.ToList() select c;
            var roles = from c in _roleManager.Roles.ToList() select c;

            model.UserId = users.First().Id;
            model.RoleId = roles.First().Id;

            model.Users = new SelectList(users, "Id", "UserName");
            model.Roles = new SelectList(roles, "Id", "Name");
        }
    }
}
