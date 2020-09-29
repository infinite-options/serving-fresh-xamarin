namespace InfiniteMeals
{
    internal class Post
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

    }

    internal class OrderPost
    {
        public string item_name { get; set; }
        public string item_qty { get; set; }
    }
}