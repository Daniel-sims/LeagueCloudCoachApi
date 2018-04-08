namespace LccWebAPI.Models.StaticData
{
    public class Item
    {
        // Primary Key
        public int Id { get; set; }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImageFull { get; set; }

        public string PlainText { get; set; }

        public string Description { get; set; }
        public string SanitizedDescription { get; set; }
    }
}
