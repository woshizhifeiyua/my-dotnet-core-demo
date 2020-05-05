using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoDALUser.Model.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Account { get; set; }
        [MaxLength(30), Required]
        public string Password { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
