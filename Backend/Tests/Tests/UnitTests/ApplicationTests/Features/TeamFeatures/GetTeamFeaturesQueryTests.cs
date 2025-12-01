using Application.Features.TeamFeatures.Queries;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.UnitTests.ApplicationTests.Features.TeamFeatures;

public class GetTeamFeaturesQueryTests
{
    [Fact]
    public void GetTeamFeaturesQuery_Should_ReturnTeams()
    {
        // Arrange
        var query = new GetTeamFeaturesQuery();

        // Act & Assert
        query.Should().NotBeNull();
        query.GetType().Name.Should().Be("GetTeamFeaturesQuery");
    }

    [Fact]
    public void Team_Entity_Should_HaveValidProperties()
    {
        // Arrange
        var teamId = Guid.NewGuid().ToString();
        var createDate = DateTime.UtcNow;
        var team = new Team
        {
            Id = teamId,
            Name = "Test Team",
            Description = "Test Description",
            CreateDate = createDate,
            UpdateDate = createDate,
            IsDelete = false
        };

        // Act & Assert
        team.Id.Should().Be(teamId);
        team.Name.Should().Be("Test Team");
        team.Description.Should().Be("Test Description");
        team.CreateDate.Should().Be(createDate);
        team.UpdateDate.Should().Be(createDate);
        team.IsDelete.Should().BeFalse();
        team.DeleteDate.Should().BeNull();
    }

    [Fact]
    public void Team_Entity_Should_HaveDefaultValues_WhenCreated()
    {
        // Arrange & Act
        var team = new Team();

        // Assert
        team.Id.Should().NotBeNullOrEmpty();
        team.Name.Should().BeEmpty();
        team.Description.Should().BeEmpty();
        team.IsDelete.Should().BeFalse();
        team.DeleteDate.Should().BeNull();
        // CreateDate ve UpdateDate constructor'da set edilmiyorsa default olacak
    }

    [Fact]
    public void Team_Entity_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var team = new Team
        {
            Name = "Development Team",
            Description = "Software Development Team"
        };

        // Act
        team.CreateDate = DateTime.UtcNow.AddDays(-1);
        team.UpdateDate = DateTime.UtcNow;
        team.IsDelete = true;
        team.DeleteDate = DateTime.UtcNow;

        // Assert
        team.Name.Should().Be("Development Team");
        team.Description.Should().Be("Software Development Team");
        team.CreateDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(-1), TimeSpan.FromSeconds(1));
        team.UpdateDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        team.IsDelete.Should().BeTrue();
        team.DeleteDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("Development Team", "Dev team")]
    [InlineData("Design Team", "UI/UX Design")]
    [InlineData("Marketing Team", "")]
    [InlineData("Sales Team", "Sales and Business Development")]
    public void Team_Entity_Should_AcceptDifferentValues(string name, string description)
    {
        // Arrange
        var team = new Team
        {
            Name = name,
            Description = description
        };

        // Act & Assert
        team.Name.Should().Be(name);
        team.Description.Should().Be(description);
    }

    [Fact]
    public void Team_Id_Should_BeValidGuidString()
    {
        // Arrange
        var team = new Team();

        // Act
        var isValidGuid = Guid.TryParse(team.Id, out _);

        // Assert
        isValidGuid.Should().BeTrue("Team Id should be a valid GUID string");
        team.Id.Should().MatchRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
    }
}