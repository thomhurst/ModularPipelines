using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "commitments", "update")]
public record GcloudComputeCommitmentsUpdateOptions(
[property: PositionalArgument] string Commitment
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}