﻿using WebApi.Models;

namespace Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }    

        public virtual List<FastFoodItem> FastFoodItems { get; set; }
    }
}
