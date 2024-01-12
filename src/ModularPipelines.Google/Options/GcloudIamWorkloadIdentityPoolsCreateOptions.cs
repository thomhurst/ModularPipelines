using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "create")]
public record GcloudIamWorkloadIdentityPoolsCreateOptions(
[property: PositionalArgument] string WorkloadIdentityPool,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}