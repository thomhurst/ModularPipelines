using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "operations", "list")]
public record GcloudAppOperationsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--pending")]
    public bool? Pending { get; set; }
}