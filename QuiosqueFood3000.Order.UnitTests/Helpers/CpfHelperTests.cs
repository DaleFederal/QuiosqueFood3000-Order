using QuiosqueFood3000.Api.Helpers;

namespace QuiosqueFood3000.Order.UnitTests.Helpers;

public class CpfHelperTests
{
    [Theory]
    [InlineData("12345678901", false)]
    [InlineData("11111111111", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("123", false)]
    public void IsValidCpf_ShouldReturnExpectedResult(string cpf, bool expected)
    {
        // Act
        var result = CpfHelper.IsValidCpf(cpf);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123.456.789-01", "12345678901")]
    [InlineData(" 123.456.789-01 ", "12345678901")]
    [InlineData(null, "")]
    public void RemoveSpecialCaracters_ShouldRemoveSpecialCharacters(string input, string expected)
    {
        // Act
        var result = CpfHelper.RemoveSpecialCaracters(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
