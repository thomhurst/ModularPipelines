using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "account", "login")]
public record AzBatchAccountLoginOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--shared-key-auth")]
    public bool? SharedKeyAuth { get; set; }

    [CliFlag("--show")]
    public bool? Show { get; set; }
}