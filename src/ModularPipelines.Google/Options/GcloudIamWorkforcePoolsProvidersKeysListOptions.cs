using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "keys", "list")]
public record GcloudIamWorkforcePoolsProvidersKeysListOptions(
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--workforce-pool")] string WorkforcePool
) : GcloudOptions
{
    [BooleanCommandSwitch("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}