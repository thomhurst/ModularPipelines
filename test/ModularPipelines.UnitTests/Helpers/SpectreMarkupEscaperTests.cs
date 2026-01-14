using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Tests for <see cref="SpectreMarkupEscaper"/>.
/// </summary>
public class SpectreMarkupEscaperTests
{
    [Test]
    public async Task Escape_WithNullInput_ReturnsEmptyString()
    {
        // Act
        var result = SpectreMarkupEscaper.Escape(null);

        // Assert
        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Escape_WithEmptyString_ReturnsEmptyString()
    {
        // Act
        var result = SpectreMarkupEscaper.Escape(string.Empty);

        // Assert
        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Escape_WithNoSpecialCharacters_ReturnsSameString()
    {
        // Arrange
        const string input = "MyModule";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(input);
    }

    [Test]
    public async Task Escape_WithOpenBracket_EscapesBracket()
    {
        // Arrange
        const string input = "Module[Test]";
        const string expected = "Module[[Test]]";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Escape_WithCloseBracket_EscapesBracket()
    {
        // Arrange
        const string input = "Module]Test";
        const string expected = "Module]]Test";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Escape_WithMultipleBrackets_EscapesAll()
    {
        // Arrange
        const string input = "[Module[Test]Output]";
        const string expected = "[[Module[[Test]]Output]]";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Escape_WithSpectreMarkupLikeSyntax_EscapesProperly()
    {
        // This simulates a module name that looks like Spectre markup
        // Arrange
        const string input = "[green]SomeModule[/]";
        const string expected = "[[green]]SomeModule[[/]]";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Escape_WithGenericTypeSyntax_EscapesProperly()
    {
        // Generic type names might have angle brackets but no square brackets
        // This test ensures we only escape square brackets
        // Arrange
        const string input = "Module<T>";

        // Act
        var result = SpectreMarkupEscaper.Escape(input);

        // Assert
        await Assert.That(result).IsEqualTo(input);
    }
}
