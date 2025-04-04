using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.EN
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [ForeignKey("Cliente")]

        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El total de la compra es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El total debe ser mayor a 0 y menor a 1000")]
        [Column(TypeName = "decimal(10,2)")]

        public decimal Total { get; set; }

        public byte Estado { get; set; }

        public virtual Cliente? Cliente { get; set; }

        public virtual ICollection<DetalleVenta>? DetalleVentas { get; set; }

        public enum EnumEstadoVentas
        {
            Activa = 1,
            Anulada = 2,
        }
    }
}
