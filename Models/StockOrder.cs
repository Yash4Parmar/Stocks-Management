using System;
using System.Collections.Generic;

namespace Stocks_Management.Models;

public partial class StockOrder
{
    public int Id { get; set; }

    public int Sid { get; set; }

    public int Oid { get; set; }

    public virtual Order OidNavigation { get; set; } = null!;

    public virtual Stock SidNavigation { get; set; } = null!;
}
