using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }


        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
        public decimal Iva { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
