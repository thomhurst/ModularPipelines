using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "approuting", "enable")]
public record AzAksApproutingEnableOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--attach-kv")]
    public bool? AttachKv { get; set; }

    [CliFlag("--enable-kv")]
    public bool? EnableKv { get; set; }
}