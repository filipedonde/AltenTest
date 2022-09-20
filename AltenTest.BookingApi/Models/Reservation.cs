namespace AltenTest.BookingApi.Models
{
    public class Reservation
    {
        public int ReservationNumber { get; set; }
        public string Guest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCancelled { get; set; }
    }
}
