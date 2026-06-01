using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolLibrary.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public ApplicationUser Reader { get; set; } = null!;
        public string ReaderId { get; set; } = string.Empty;

        public LibraryItem LibraryItem { get; set; } = null!;
        public int LibraryItemId { get; set; }

        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
