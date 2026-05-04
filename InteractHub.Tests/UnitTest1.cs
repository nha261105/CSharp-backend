namespace InteractHub.Tests;

public class BasicTests
{
    [Fact]
    public void SampleTest_AlwaysPasses()
    {
        // Arrange
        var expected = 5;
        var actual = 2 + 3;

        // Act & Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void StringTest_ChecksEquality()
    {
        // Arrange
        var greeting = "Hello";

        // Act
        var result = greeting + " World";

        // Assert
        Assert.Equal("Hello World", result);
    }
}