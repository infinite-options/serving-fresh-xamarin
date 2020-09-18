using System;
using System.Collections.Generic;

namespace InfiniteMeals.Meals.Model
{
    public class Items
    {
        public string item_uid { get; set; }
        public string created_at { get; set; }
        public string itm_business_uid { get; set; }
        public string item_name { get; set; }
        public string item_desc { get; set; }
        public double item_price { get; set; }
        public object item_sizes { get; set; }
        public string item_photo { get; set; }
        public string favorite { get; set; }
        public string info_ { get; set; }
    }

    public class Result
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Items> result { get; set; }
        public string sql { get; set; }
    }
    
    public class ServingFreshBusinessItems
    {
        public string message { get; set; }
        public Result result { get; set; }
    }
}
