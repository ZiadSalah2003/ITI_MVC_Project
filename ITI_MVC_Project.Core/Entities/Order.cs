using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core.Entities
{
	public class Order
	{
		public int OrderId { get; set; }
		public string UserId { get; set; }
		public int ShippingAddressId { get; set; }
		public Address ShippingAddress { get; set; }
		public string OrderNumber { get; set; }
		public int Status { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public ApplicationUser User { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
}
