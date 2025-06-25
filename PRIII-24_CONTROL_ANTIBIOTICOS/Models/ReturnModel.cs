namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class ReturnModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
