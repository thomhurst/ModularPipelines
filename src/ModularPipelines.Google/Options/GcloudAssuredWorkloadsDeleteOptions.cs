using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "delete")]
public record GcloudAssuredWorkloadsDeleteOptions(
[property: PositionalArgument] string Workload,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}