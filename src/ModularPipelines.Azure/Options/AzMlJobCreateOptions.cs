using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "job", "create")]
public record AzMlJobCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--save-as")]
    public string? SaveAs { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-validation")]
    public bool? SkipValidation { get; set; }

    [CliFlag("--stream")]
    public bool? Stream { get; set; }

    [CliFlag("--web")]
    public bool? Web { get; set; }
}