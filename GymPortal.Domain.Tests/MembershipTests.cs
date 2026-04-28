using GymPortal.Domain.Entities;
using GymPortal.Domain.Enums;

namespace GymPortal.Domain.Tests;

public class MembershipTests
{
    [Fact]
    public void CreateMembership_Should_Set_Active_Status()
    {
        var userId = "user-1";

        var membership = new Membership(userId, MembershipType.Standard);

        Assert.Equal(MembershipStatus.Active, membership.Status);
        Assert.Equal(userId, membership.UserId);
        Assert.NotEqual(Guid.Empty, membership.Id);
    }

    [Fact]
    public void CancelMembership_Should_Set_Cancelled_Status()
    {
        var membership = new Membership("user-1", MembershipType.Standard);

        membership.Cancel();

        Assert.Equal(MembershipStatus.Cancelled, membership.Status);
        Assert.NotNull(membership.EndDate);
    }
}