using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medical_Center.ViewModel
{
    public class RegisterDoctors
    {
        //Required attribute implements validation on Model item that this fields is mandatory for user
        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "AMKA is not valid")]
        [Display(Name = "AMKA")]
        public string AMKA { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Speciality")]
        public string Speciality { get; set; }

        [Required]
        [Display(Name = "Admin id")]
        public string Admin_id { get; set; }
    }
}