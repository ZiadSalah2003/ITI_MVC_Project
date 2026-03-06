using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core.Entities
{
	public class OrderItem
	{
		public int OrderItemId { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal LineTotal { get; set; }
		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}
