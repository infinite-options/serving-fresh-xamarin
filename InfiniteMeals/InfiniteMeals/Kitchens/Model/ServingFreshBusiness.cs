using System;
using System.Collections.Generic;

namespace InfiniteMeals.Kitchens.Model
{
    
    public class Bussiness
    {
        public string business_uid { get; set; }
        public DateTime? business_created_at { get; set; }
        public string business_name { get; set; }
        public string business_type { get; set; }
        public string business_desc { get; set; }
        public string business_contact_first_name { get; set; }
        public string business_contact_last_name { get; set; }
        public string business_phone_num { get; set; }
        public string business_phone_num2 { get; set; }
        public string business_email { get; set; }
        public string business_hours { get; set; }
        public string business_accepting_hours { get; set; }
        public string business_delivery_hours { get; set; }
        public string business_address { get; set; }
        public string business_unit { get; set; }
        public string business_city { get; set; }
        public string business_state { get; set; }
        public string business_zip { get; set; }
        public object business_longitude { get; set; }
        public object business_latitude { get; set; }
        public object business_EIN { get; set; }
        public object business_WAUBI { get; set; }
        public object business_license { get; set; }
        public object business_USDOT { get; set; }
        public object notification_approval { get; set; }
        public object notification_device_id { get; set; }
        public int? can_cancel { get; set; }
        public int? delivery { get; set; }
        public int? reusable { get; set; }
        public string business_image { get; set; }
        public string business_password { get; set; }
    }

        public class Result
        {
            public string message { get; set; }
            public int code { get; set; }
            public IList<Bussiness> result { get; set; }
            public string sql { get; set; }
        }

        public class ServingFreshBusiness
        {
            public string message { get; set; }
            public Result result { get; set; }
        }
    
}
