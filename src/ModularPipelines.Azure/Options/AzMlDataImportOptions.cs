using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "data", "import")]
public record AzMlDataImportOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--datastore")]
    public string? Datastore { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-validation")]
    public bool? SkipValidation { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}