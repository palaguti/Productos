using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.EN
{
    public class Cliente
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente esobligatorio ")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener mas de 255 caracteres")]
        public string Nombre { get; set; }

        [StringLength(20, ErrorMessage = "El telefono no puede tener mas de 20 caracteres")]
        public string? Telefono { get; set; }

        [StringLength(100, ErrorMessage = "El correo electronico no puede tener mas de 100 caracteres")]
        [EmailAddress(ErrorMessage = "El correo electronico no tiene un formato valido")]

        public string? Correo { set; get; }
    }
}
