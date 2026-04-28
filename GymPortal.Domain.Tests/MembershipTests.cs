using GymPortal.Domain.Entities;
using GymPortal.Domain.Enums;

namespace GymPortal.Domain.Tests;

public class MembershipTests
{
    // Dessa tester verifierar att medlemskap skapas med rätt status och att avbokning av medlemskap uppdaterar status och slutdatum korrekt.
    [Fact]
    public void CreateMembership_Should_Set_Active_Status()
    {
        var userId = "user-1"; // Skapa ett medlemskap för en användare

        var membership = new Membership(userId, MembershipType.Standard); // Skapa ett medlemskap

        // Verifiera att medlemskapet har rätt status, användar-ID och att ett unikt ID har genererats.
        Assert.Equal(MembershipStatus.Active, membership.Status);
        Assert.Equal(userId, membership.UserId);
        Assert.NotEqual(Guid.Empty, membership.Id);
    }

    // Testar att avbokning av medlemskap sätter status till "Cancelled" och uppdaterar slutdatum korrekt.
    [Fact]
    public void CancelMembership_Should_Set_Cancelled_Status()
    {
        var membership = new Membership("user-1", MembershipType.Standard); // Skapa ett medlemskap

        membership.Cancel(); // Avboka medlemskapet

        // Verifiera att medlemskapet har status "Cancelled" och att slutdatum har satts.
        Assert.Equal(MembershipStatus.Cancelled, membership.Status);
        Assert.NotNull(membership.EndDate);
    }
}