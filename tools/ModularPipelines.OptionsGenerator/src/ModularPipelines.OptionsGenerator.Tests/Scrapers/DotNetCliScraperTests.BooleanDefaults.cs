using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class DotNetCliScraperTests
{
    [Test]
    [Arguments("--include-references", "True", false)]
    [Arguments("--include-references", "true", false)]
    [Arguments("--create-manifest-if-needed", "True", false)]
    [Arguments("--create-manifest-if-needed <BOOL>", "True", false)]
    [Arguments("--include-references", "False", true)]
    public async Task Boolean_Defaults_Preserve_Explicit_Values_Through_Enhancement_And_Generation(
        string declaration, string defaultValue, bool expectedFlag)
    {
        var help = $"Options:\n  {declaration}  Configure this option. [default: {defaultValue}]\n";
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "solution", "add"], help))!;
        var parsed = command.Options.Single();
        await Assert.That(parsed.IsFlag).IsEqualTo(expectedFlag);
        await Assert.That(parsed.CSharpType).IsEqualTo("bool?");

        var enhanced = await OptionTypeEnhancer.CreateDefault(new DotNet11HelpExecutor(help), NullLoggerFactory.Instance)
            .EnhanceAsync(DotNet11Tool(command));
        var option = enhanced.Commands.Single().Options.Single();
        await Assert.That(option.IsFlag).IsEqualTo(expectedFlag);
        await Assert.That(option.PropertyType).IsEqualTo("bool?");

        var generated = (await new OptionsClassGenerator().GenerateAsync(enhanced)).Single().Content;
        await Assert.That(generated).Contains($"public bool? {option.PropertyName}");
        await Assert.That(generated).Contains($"[{(expectedFlag ? "CliFlag" : "CliOption")}(\"{option.SwitchName}\"");
    }
}
