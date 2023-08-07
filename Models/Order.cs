using System.ComponentModel.DataAnnotations;

namespace AmarraderoLlanero.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CurrentDate { get; set; }
        public string PedidoAsadero { get; set; }
        public int CantPedidoAsadero { get; set; }
        public string PedidoCocina { get; set; }
        public int CantPedidoCocina { get; set; }
        public string PedidoBar { get; set; }
        public int CantPedidoBar { get; set; }
        
    }
}
