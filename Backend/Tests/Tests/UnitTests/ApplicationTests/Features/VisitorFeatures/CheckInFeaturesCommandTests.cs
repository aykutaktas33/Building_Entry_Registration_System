using Application.Features.VisitorFeatures.Commands;
using Application.Features.VisitorFeatures.Commands.Validators;
using Application.Responses.VisitorFeatures;
using FluentAssertions;
using Xunit;

namespace Tests.UnitTests.ApplicationTests.Features.VisitorFeatures;

public class CheckInFeaturesCommandTests
{
    private readonly CheckInFeaturesCommandValidator _validator;

    public CheckInFeaturesCommandTests()
    {
        _validator = new CheckInFeaturesCommandValidator();
    }

    [Fact]
    public void CheckInFeaturesCommand_Should_RequireSessionId()
    {
        // Arrange
        var command = new CheckInFeaturesCommand
        {
            SessionId = "session_123"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        command.SessionId.Should().Be("session_123");
    }

    [Theory]
    [InlineData("session_123", true)]
    [InlineData("sess_abc456", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("  ", false)] // Whitespace only
    public void CheckInFeaturesCommandValidator_Should_ValidateSessionId(
        string sessionId, bool expectedIsValid)
    {
        // Arrange
        var command = new CheckInFeaturesCommand
        {
            SessionId = sessionId
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void CheckInResponse_Should_HaveValidProperties()
    {
        // Arrange
        var checkInTime = DateTime.UtcNow;
        var localTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var response = new CheckInResponse
        {
            VisitorId = "VISITOR-001",
            SessionId = "session_123",
            Name = "John Doe",
            Email = "john@example.com",
            Company = "Tech Corp",
            TeamName = "Development Team",
            EntranceId = "ENTRANCE-001",
            CheckInTime = checkInTime,
            LocalCheckInTime = localTime
        };

        // Act & Assert
        response.VisitorId.Should().Be("VISITOR-001");
        response.SessionId.Should().Be("session_123");
        response.Name.Should().Be("John Doe");
        response.Email.Should().Be("john@example.com");
        response.Company.Should().Be("Tech Corp");
        response.TeamName.Should().Be("Development Team");
        response.EntranceId.Should().Be("ENTRANCE-001");
        response.CheckInTime.Should().Be(checkInTime);
        response.LocalCheckInTime.Should().Be(localTime);
    }

    [Fact]
    public void CheckInResponse_Should_HaveDefaultValues()
    {
        // Arrange & Act
        var response = new CheckInResponse();

        // Assert
        response.VisitorId.Should().BeEmpty();
        response.SessionId.Should().BeEmpty();
        response.Name.Should().BeEmpty();
        response.Email.Should().BeEmpty();
        response.Company.Should().BeEmpty();
        response.TeamName.Should().BeEmpty();
        response.EntranceId.Should().BeEmpty();
        response.CheckInTime.Should().Be(default);
        response.LocalCheckInTime.Should().BeEmpty();
    }

    [Fact]
    public void CheckInResponse_CheckInTime_Should_BeUtc()
    {
        // Arrange
        var response = new CheckInResponse
        {
            CheckInTime = DateTime.UtcNow
        };

        // Act & Assert
        response.CheckInTime.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("VISITOR-001", "John Doe", "john@example.com")]
    [InlineData("VISITOR-002", "Jane Smith", "jane@example.com")]
    public void CheckInResponse_Should_AcceptDifferentVisitorData(
        string visitorId, string name, string email)
    {
        // Arrange
        var response = new CheckInResponse
        {
            VisitorId = visitorId,
            Name = name,
            Email = email,
            Company = "Test Company",
            TeamName = "Test Team",
            EntranceId = "ENTRANCE-001",
            CheckInTime = DateTime.UtcNow,
            LocalCheckInTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Act & Assert
        response.VisitorId.Should().Be(visitorId);
        response.Name.Should().Be(name);
        response.Email.Should().Be(email);
        response.Company.Should().Be("Test Company");
        response.TeamName.Should().Be("Test Team");
        response.EntranceId.Should().Be("ENTRANCE-001");
    }
}