using QuiosqueFood3000.Api.Helpers;

namespace QuiosqueFood3000.Order.UnitTests.Helpers;

public class EmailHelperTests
{
    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidEmail_ShouldReturnExpectedResult(string email, bool expected)
    {
        // Act
        var result = EmailHelper.IsValidEmail(email);

        // Assert
        Assert.Equal(expected, result);
    }
}
