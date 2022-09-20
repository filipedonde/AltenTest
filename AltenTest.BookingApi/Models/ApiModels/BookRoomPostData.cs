namespace AltenTest.BookingApi.Models.ApiModels
{
    public class BookRoomPostData
    {
        public string? Guest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
