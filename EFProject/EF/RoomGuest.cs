using System;
using System.Collections.Generic;

namespace EFProject.EF;

public partial class RoomGuest
{
    public int IdRoomGuest { get; set; }

    public int IdRoom { get; set; }

    public int IdGuest { get; set; }

    public int LengthOfStay { get; set; }

    public virtual Guest IdGuestNavigation { get; set; } = null!;
}
