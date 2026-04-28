using GymPortal.Domain.Entities;

namespace GymPortal.Tests;

public class BookingTests
{
    // Dessa tester verifierar att en bokning skapas med rätt användar-ID, gympass-ID, och att ett unikt ID genereras samt att bokningstidpunkten är korrekt.
    [Fact]
    public void CreateBooking_Should_Set_Values_Correctly()
    {
        var userId = "user-1";
        var classId = Guid.NewGuid();

        var booking = new Booking(userId, classId);

        // Verifiera att bokningen har rätt värden
        Assert.Equal(userId, booking.UserId);
        Assert.Equal(classId, booking.GymClassId);
        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.True(booking.BookedAt <= DateTime.UtcNow);
    }
}