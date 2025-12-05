using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "custom-target-types", "list")]
public record GcloudDeployCustomTargetTypesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}