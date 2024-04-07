using ApiAppBangHang.Models;
using System;
using System.Collections.Generic;

namespace ApiAppBanSach.Models
{
    public class AppUser
    {
        //public string Id { get; set; }
        public string AppUserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public DateTime Updated { get; set; }
        public List<UserAddress> UserAddresses { get; set; }
        public Cart Cart { get; set; }
    }
}
