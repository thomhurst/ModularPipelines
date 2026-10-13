using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SynopsisIdentifierRegressionTests
{
    [Test]
    [Arguments("404-document", "Number404Document")]
    [Arguments("9p", "Number9p")]
    [Arguments("123", "Number123")]
    public async Task Leading_Digits_Have_A_Readable_Prefix(string input, string expected)
    {
        await Assert.That(GeneratorUtils.ToPascalCase(input)).IsEqualTo(expected);
    }
}
