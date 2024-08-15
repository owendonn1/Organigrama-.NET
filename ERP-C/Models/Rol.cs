using ERP_C.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ERP_C.Models
{
    public class Rol : IdentityRole<int>
    {
        #region Constructores

        public Rol() : base() { }
        public Rol(string Name) : base(Name) { }

        #endregion

        #region Propiedades

        [Display(Name = Alias.RoleName)] 
        public string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public override string NormalizedName 
        {
            get => base.NormalizedName;
            set => base.NormalizedName = value; 
        }
        #endregion


    }
}
