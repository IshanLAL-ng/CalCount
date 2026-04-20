namespace CalCount.Models
{
    /// <summary>
    /// Represents water intake logging
    /// </summary>
    public class WaterLog
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Amount of water in milliliters
        /// </summary>
        public double AmountMl { get; set; }
        
        /// <summary>
        /// Time when water was consumed
        /// </summary>
        public DateTime LoggedDateTime { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
    }
}
