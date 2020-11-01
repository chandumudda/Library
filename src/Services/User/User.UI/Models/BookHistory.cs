using System;

namespace UserUI.Models
{
    public class BookHistory
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string ReadyBy { get; set; }
        public DateTime ReadDate { get; set; }
    }
}
