using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzPositionalArgumentTests
{
    [Test]
    [Arguments("set", "KeyValue", "IEnumerable<string>", true, true)]
    [Arguments("get", "Key", "string?", false, false)]
    [Arguments("unset", "Key", "IEnumerable<string>", true, true)]
    [Arguments("param-persist delete", "Name", "IEnumerable<string>?", false, true)]
    public async Task Config_Positionals_Preserve_Parser_Arity(
        string command, string propertyName, string type, bool required, bool variadic)
    {
        var parsed = await ParseFixture(command);
        var argument = parsed.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo(propertyName);
        await Assert.That(argument.CSharpType).IsEqualTo(type);
        await Assert.That(argument.IsRequired).IsEqualTo(required);
        await Assert.That(argument.IsVariadic).IsEqualTo(variadic);
        await Assert.That(argument.PositionIndex).IsEqualTo(0);
        await Assert.That(argument.Description).IsNotNull();
        await Assert.That(parsed.Options.Any(option => option.PropertyName == propertyName)).IsFalse();
    }

    [Test]
    public async Task Wrapped_Positional_Description_Stops_At_Next_Section()
    {
        var argument = (await ParseFixture("get")).PositionalArguments.Single();
        await Assert.That(argument.Description).Contains("corresponding configuration is shown.");
        await Assert.That(argument.Description).DoesNotContain("Global Arguments");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Unverified_Positionals_Stop_Generation(bool metadataPresent)
    {
        var help = "Command\n    az config set : Set a configuration.\nPositional\n    <KEY_VALUE> : Configuration values.\n";
        if (metadataPresent)
        {
            help += AzCliMetadataExecutor.MetadataMarker + "{}";
        }

        async Task Act() => _ = await new Scraper().Parse(["az", "config", "set"], help);
        await Assert.That(Act).Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments("<KEY VALUE> : Unsupported placeholder.", false)]
    [Arguments("<KEY> = Unsupported separator.", false)]
    [Arguments("<KEY VALUE> : Unsupported placeholder.", true)]
    [Arguments("<KEY> = Unsupported separator.", true)]
    public async Task Malformed_Positional_Rows_Stop_Generation(string row, bool afterValidRow)
    {
        var help = "Command\n    az example : Demonstrate operands.\nArguments\n    --local : Use local settings.\nPositional\n";
        if (afterValidRow)
        {
            help += "    <RESOURCE> : Resource name.\n                 " + row + "\n";
        }
        else
        {
            help += "    " + row + "\n";
        }

        help += AzCliMetadataExecutor.MetadataMarker
            + "{\"--local\":{\"Nargs\":\"0\",\"IsInteger\":false,\"IsRepeated\":false},\"resource\":{\"Nargs\":\"1\",\"IsInteger\":false,\"IsRepeated\":false}}";
        async Task Act() => _ = await new Scraper().Parse(["az", "example"], help);
        await Assert.That(Act).Throws<InvalidOperationException>()
            .And.HasMessageContaining("Unrecognized Azure CLI positional row");
    }

    [Test]
    [Arguments("Required")]
    [Arguments("Experimental")]
    [Arguments("Preview")]
    public async Task Annotated_Positionals_Preserve_Verified_Arity(string annotation)
    {
        var help = $"Command\n    az example : Demonstrate operands.\nPositional\n    <KEY> [{annotation}] : Key name.\n"
            + AzCliMetadataExecutor.MetadataMarker
            + "{\"key\":{\"Nargs\":\"1\",\"IsInteger\":false,\"IsRepeated\":false}}";
        var command = (await new Scraper().Parse(["az", "example"], help))!;
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsTrue();
    }

    [Test]
    public async Task Storage_Passthrough_Positionals_Preserve_Delimiter_And_Phase()
    {
        var argument = (await ParseStorageFixture()).PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("ExtraOptions");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.IsVariadic).IsTrue();
        await Assert.That(argument.Phase).IsEqualTo(ModularPipelines.Attributes.CommandLinePhase.Passthrough);
        await Assert.That(argument.PrependOptionTerminator).IsTrue();
    }

    internal static async Task<CliCommandDefinition> ParseStorageFixture()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "AzCli", "az-2.84.0-storage-blob-sync.txt");
        return (await new Scraper().Parse(["az", "storage", "blob", "sync"], await File.ReadAllTextAsync(path)))!;
    }

    internal static async Task<CliCommandDefinition> ParseFixture(string command)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "AzCli",
            "az-2.84.0-config-" + command.Replace(' ', '-') + ".txt");
        return (await new Scraper().Parse(["az", "config", .. command.Split(' ')], await File.ReadAllTextAsync(path)))!;
    }

    [Test]
    public async Task Positional_Only_Command_Preserves_Order_And_Integer_Types()
    {
        const string help = """
            Command
                az example : Demonstrate operands.

            Positional
                <RESOURCE> : Resource name.
                <COUNT>    : Number of copies.
            __MODULAR_PIPELINES_AZ_ARGUMENT_SHAPES__:{"resource":{"Nargs":"1","IsInteger":false,"IsRepeated":false},"count":{"Nargs":"?","IsInteger":true,"IsRepeated":false}}
            """;
        var command = (await new Scraper().Parse(["az", "example"], help))!;
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName)).IsEquivalentTo(
            ["Resource", "Count"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(command.PositionalArguments.Select(argument => argument.PositionIndex)).IsEquivalentTo(
            [0, 1], TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(command.PositionalArguments.Select(argument => argument.CSharpType)).IsEquivalentTo(
            ["string", "int?"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(command.PositionalArguments[0].Description).IsEqualTo("Resource name.");
    }

    private sealed class Scraper() : AzCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, CancellationToken.None);
    }
}
