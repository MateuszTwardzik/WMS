namespace MagazynApp.Models
{
    public class SocketProduct
    {
        public int SocketId { get; set; }
        public Socket Socket { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Amount { get; set; }
    }
}