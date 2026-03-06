using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core.Entities
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public int? ParentCategory { get; set; }
		public ICollection<Product> Products { get; set; } = new HashSet<Product>();
	}
}
