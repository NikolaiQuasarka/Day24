using System;
using System.Collections.Generic;

namespace EFProject.EF;

public partial class Room
{
    public int IdRoom { get; set; }

    public int Number { get; set; }

    public int? IdGuest { get; set; }

    public int GuestCount { get; set; }

    public int ComfortLevel { get; set; }

    public int Price { get; set; }

    public virtual Guest? IdGuestNavigation { get; set; }
}
