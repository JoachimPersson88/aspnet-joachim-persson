using GymPortal.Domain.Entities;

namespace GymPortal.Domain.Tests;

public class GymClassTests
{
    // Detta test verifierar att en GymClass skapas med rätt värden och att ett unikt ID genereras.
    [Fact]
    public void CreateGymClass_Should_Set_Values_Correctly()
    {
        var startTime = new DateTime(2026, 5, 5, 18, 0, 0);

        var gymClass = new GymClass("Yoga", "Rörlighet", startTime, 20, "Emma");

        Assert.Equal("Yoga", gymClass.Name);
        Assert.Equal("Rörlighet", gymClass.Category);
        Assert.Equal(startTime, gymClass.StartTime);
        Assert.Equal(20, gymClass.Capacity);
        Assert.Equal("Emma", gymClass.Instructor);
        Assert.NotEqual(Guid.Empty, gymClass.Id);
    }
}