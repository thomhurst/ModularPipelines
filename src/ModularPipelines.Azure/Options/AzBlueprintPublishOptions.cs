using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "publish")]
public record AzBlueprintPublishOptions(
[property: CliOption("--blueprint-name")] string BlueprintName,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--change-notes")]
    public string? ChangeNotes { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}