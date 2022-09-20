using AltenTest.BookingApi.Application.Commands;
using AltenTest.BookingApi.Application.Handlers;
using AltenTest.BookingApi.Data.Repository;
using AltenTest.BookingApi.Models;
using AltenTest.Core.Data;
using Moq;

namespace AltenTest.Tests.BookingApi.Commands
{
    [TestClass]
    public class BookRoomTests
    {
        private const string MORE_THAN_3_DAYS_MESSAGE = "The stay can't be longer than 3 days.";
        private const string NOT_AVAILABLE_MESSAGE = "The room is not available in the selected dates.";


        private BookRoomCommandHandler _handler;
        private Mock<IReservationRepository> _mReservationRepository;
        private Mock<IUnitOfWork> _mUnitOfWork;


        [TestInitialize]
        public void Init()
        {
            _mReservationRepository = new Mock<IReservationRepository>();
            _mUnitOfWork = new Mock<IUnitOfWork>();
            _mUnitOfWork.Setup(x => x.Commit()).Returns(Task.FromResult(true));

            _mReservationRepository.Setup(x => x.UnitOfWork).Returns(_mUnitOfWork.Object);
            _mReservationRepository.Setup(r => r.IsAvailable(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3))).Returns(Task.FromResult(true));

            _handler = new BookRoomCommandHandler(_mReservationRepository.Object);
        }

        [TestMethod]
        public async Task BookRoom_HappyWay()
        {
            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)), CancellationToken.None);

            Assert.IsTrue(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Once());
            _mUnitOfWork.Verify(x => x.Commit());
        }

        [TestMethod]
        public async Task BookRoom_CannotBookRoomIsNotAvailable()
        {
            _mReservationRepository.Setup(r => r.IsAvailable(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3))).Returns(Task.FromResult(false));

            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);
            Assert.AreEqual(NOT_AVAILABLE_MESSAGE, result.ValidationResult.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task BookRoom_CannotStayLongerThan3Days()
        {
            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(1), DateTime.Today.AddDays(4)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);
            Assert.AreEqual(MORE_THAN_3_DAYS_MESSAGE, result.ValidationResult.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task BookRoom_CannotBookRoomInTheCurrentDay()
        {
            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today, DateTime.Today.AddDays(2)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);
        }

        [TestMethod]
        public async Task BookRoom_CannotBookRoomInThePast()
        {
            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);
        }

        [TestMethod]
        public async Task BookRoom_CannotBookRoomWith30DaysInAdvance()
        {
            var result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(29), DateTime.Today.AddDays(31)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);


            result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(30), DateTime.Today.AddDays(32)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);

            result = await _handler.Handle(new BookRoomCommand("Filipe", DateTime.Today.AddDays(31), DateTime.Today.AddDays(33)), CancellationToken.None);

            Assert.IsFalse(result.IsSucess);
            _mReservationRepository.Verify(x => x.Add(It.IsAny<Reservation>()), Times.Never());
            _mUnitOfWork.Verify(x => x.Commit(), Times.Never);
        }
    }
}
