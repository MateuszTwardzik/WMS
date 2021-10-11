namespace MagazynApp.Models
{
    public class Converter
    {
        public int Id { get; set; }
        public string MeasureIn { get; set; }
        public  string MeasureOut { get; set; }
        public double Value { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}