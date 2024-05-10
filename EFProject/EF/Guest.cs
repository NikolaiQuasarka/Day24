using System;
using System.Collections.Generic;

namespace EFProject.EF;

public partial class Guest
{
    public int IdGuest { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public string Gender { get; set; } = null!;

    public byte[]? FaceImage { get; set; }

    public virtual ICollection<RoomGuest> RoomGuests { get; set; } = new List<RoomGuest>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
