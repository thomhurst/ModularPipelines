using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("[--names <value> [<value>...]]", "list", false)]
    [Arguments("--names <value> [<value>...]", "list", true)]
    [Arguments("[--names <value> [<value>...]]", "string", false)]
    [Arguments("[--names <value>\n       [<value>...]]", "string", false)]
    public async Task Repeated_Option_Metavariables_Do_Not_Create_Operands(string synopsis, string type, bool required)
    {
        var scraper = new TestAwsCliScraper($"""
            SYNOPSIS
                   aws fixture apply
                   {synopsis}
            OPTIONS
                   --names ({type})
                    Names to filter by.
            """);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var parsed = commands.Single();
        await Assert.That(parsed.PositionalArguments).IsEmpty();
        await Assert.That(parsed.RequiredAlternativeGroups).IsEmpty();
        var names = parsed.Options.Single();
        await Assert.That(names.AcceptsMultipleValues).IsTrue();
        await Assert.That(names.GroupValues).IsTrue();
        await Assert.That(names.IsRequired).IsEqualTo(required);
        await Assert.That(names.IsScalarValue).IsFalse();
        var generated = await new OptionsClassGenerator().GenerateAsync(scraper.CreateToolDefinition() with { Commands = [parsed] });
        var content = generated.Single(file => file.RelativePath.EndsWith("Options.Generated.cs", StringComparison.Ordinal)).Content;
        await Assert.That(content).DoesNotContain("CliArgument(");
        await Assert.That(content).DoesNotContain("must be specified when other arguments");
    }

    [Test]
    [Arguments("group_name <value>", "string", false)]
    [Arguments("group_name <value> [<value>...]", "IEnumerable<string>", true)]
    public async Task Named_Positional_Metavariable_Describes_One_Operand(string synopsis, string type, bool variadic)
    {
        var scraper = new TestAwsCliScraper($"""
            SYNOPSIS
                   aws fixture apply
                   {synopsis}
                   [--follow]
            OPTIONS
                   group_name (string)
                    The log group name.
                   --follow (boolean)
                    Follow new logs.
            """);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var arguments = commands.Single().PositionalArguments;
        await Assert.That(arguments.Count).IsEqualTo(1);
        await Assert.That(arguments.Single().PropertyName).IsEqualTo("GroupName");
        await Assert.That(arguments.Single().IsRequired).IsTrue();
        await Assert.That(arguments.Single().IsVariadic).IsEqualTo(variadic);
        await Assert.That(arguments.Single().CSharpType).IsEqualTo(type);
    }
}
