using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments(null, null, false)]
    [Arguments("", "", false)]
    [Arguments(" ", "\t", false)]
    [Arguments("issue-id", null, true)]
    [Arguments(null, "src/example.cs", true)]
    [Arguments("issue-id", "src/example.cs", true)]
    [Arguments("", "src/example.cs", true)]
    public async Task Snyk_Ignore_Requires_An_Id_Or_File_Path(string? id, string? filePath, bool valid)
    {
        var command = await new SnykAlternativeScraper().Parse();
        var generated = await Generate(command.Options.ToList(), alternativeGroups: command.RequiredAlternativeGroups);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Id")!.SetValue(instance, id);
        options.GetProperty("FilePath")!.SetValue(instance, filePath);
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true)).IsEqualTo(valid);
        if (!valid)
        {
            await Assert.That(errors.Single().MemberNames).IsEquivalentTo(["Id", "FilePath"]);
        }
    }

    private sealed class SnykAlternativeScraper() : SnykCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<SnykCliScraper>.Instance)
    {
        public async Task<CliCommandDefinition> Parse()
        {
            const string help = """
                Options
                  --id=<ISSUE_ID>
                    Snyk ID for the issue to ignore, omitted if the ignore command used with --file-path, otherwise required.
                  --file-path=<PATH_TO_RESOURCE>
                    Filesystem for which to exclude directories or files from scanning.
                """;
            return (await ParseCommandAsync(["snyk", "ignore"], help,
                ParseUsageSynopsis(["snyk", "ignore"], help), CancellationToken.None))!;
        }
    }
}
