using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core.Entities
{
	public class Product
	{
		public int ProductId { get; set; }
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string SKU { get; set; }
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public Category Category { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
	}
}
