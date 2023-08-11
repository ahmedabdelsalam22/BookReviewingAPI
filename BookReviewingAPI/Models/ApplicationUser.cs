﻿using Microsoft.AspNetCore.Identity;

namespace BookReviewingAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
