using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }

		public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
		public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
	}
}
