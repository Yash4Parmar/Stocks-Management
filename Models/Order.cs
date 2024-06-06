﻿using System;
using System.Collections.Generic;

namespace Stocks_Management.Models;

public partial class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual ICollection<StockOrder> StockOrders { get; set; } = new List<StockOrder>();
}
