using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.EN
{
    public class Producto
    {
        public int Id { set; get; }

        [Required(ErrorMessage ="Es obligatorio el nombre de usuario ")]
        [MaxLength(100)]
        public string Nombre { set; get; }

        [Required(ErrorMessage ="El precio es obligatorio ")]
        [Range(0.01, double.MaxValue, ErrorMessage ="El precio debe ser mayor de 0")]
        public decimal Precio {  set; get; }

        [Required(ErrorMessage ="La cantidad es obligatoria")]
        [Range(1, int.MaxValue)]
        public int CantidadDisponible {  set; get; }

        [Required(ErrorMessage ="La fecha es obligatoria")]
        public DateTime FechaCreacion {  set; get; }
    }
}
