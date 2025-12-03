using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "show")]
public record AzVmssShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-user-data")]
    public bool? IncludeUserData { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }
}