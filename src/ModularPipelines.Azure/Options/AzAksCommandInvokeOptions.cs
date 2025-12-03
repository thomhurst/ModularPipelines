using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "command", "invoke")]
public record AzAksCommandInvokeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}