using Microsoft.AspNetCore.Identity;


namespace REC.Inventory.Infrastructure.Identity
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
    }
}
