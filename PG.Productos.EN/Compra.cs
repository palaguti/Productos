using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.EN
{
    public class Compra
    {
       [Key]
       public int Id { get; set; }

        [Required(ErrorMessage ="La fecha de compra es obligatoria")]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage ="El proveedor es obligatorio")]
        [ForeignKey("Proveedor")]

        public int IdProveedor { get; set; }

        [Required(ErrorMessage ="El total de la compra es obligatorio")]
        [Range( 0.01, 999999.99, ErrorMessage ="El total debe ser mayor a 0 y menor a 1000")]
        [Column(TypeName ="decimal(10,2)")]

        public decimal Total {  get; set; }

        public byte Estado {  get; set; }

        public virtual Proveedor? Proveedor { get; set; }

        public virtual ICollection<DetalleCompra>? DetalleCompras {  get; set; }

        public enum EnumEstadoCompra
        {
            Activa = 1,
            Anulada = 2,
        }

    }
}
