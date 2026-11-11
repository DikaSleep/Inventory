using System;
using System.Collections.Generic;

namespace Latihan_Project.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
