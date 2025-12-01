using Application.Features.EntranceFeatures.Queries;
using Application.Features.EntranceFeatures.Queries.Validators;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.UnitTests.ApplicationTests.Features.EntranceFeatures;

public class ValidateEntranceFeaturesQueryTests
{
    private readonly ValidateEntranceFeaturesQueryValidator _validator;

    public ValidateEntranceFeaturesQueryTests()
    {
        _validator = new ValidateEntranceFeaturesQueryValidator();
    }

    [Fact]
    public void ValidateEntranceFeaturesQuery_Should_HaveValidProperties()
    {
        // Arrange
        var query = new ValidateEntranceFeaturesQuery
        {
            EntranceId = "ENTRANCE-001"
        };

        // Act & Assert
        query.EntranceId.Should().Be("ENTRANCE-001");
    }

    [Theory]
    [InlineData("ENTRANCE-001", true)]
    [InlineData("ENTRANCE-002", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("AB", false)] // Too short
    public void ValidateEntranceFeaturesQueryValidator_Should_ValidateCorrectly(
        string entranceId, bool expectedIsValid)
    {
        // Arrange
        var query = new ValidateEntranceFeaturesQuery
        {
            EntranceId = entranceId
        };

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void ValidateEntranceFeaturesQuery_Should_ThrowBusinessException_WhenInvalid()
    {
        // Arrange
        var invalidQuery = new ValidateEntranceFeaturesQuery
        {
            EntranceId = "" // Invalid
        };

        // Act
        var validationResult = _validator.Validate(invalidQuery);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty();
    }
}