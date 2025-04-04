using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.EN
{
    public class DetalleVenta
    {
        [Key]
        public int ID { get; set; }

        public int IdVenta { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [ForeignKey("Producto")]

        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = " El precio unitario es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = " El subtotal es obligatorio")]
        [Range(0.01, 99999999.99, ErrorMessage = "decimal(10,2)")]
        [Column(TypeName = "decimal(10,2)")]

        public decimal SubTtal { get; set; }
        public virtual Venta? Venta { get; set; }
        public virtual Producto? Producto { get; set; }

    }
}
