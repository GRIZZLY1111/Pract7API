using System;
using System.Collections.Generic;

namespace Pract6.Models;

public partial class Recipe
{
    public int Id { get; set; } = -1;

    public string Name { get; set; } = null!;

    public string Cuisine { get; set; } = null!;

    public int CookingTime { get; set; }

    public int Calories { get; set; }

    public bool IsVegetarian { get; set; }
}
