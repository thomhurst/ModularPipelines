using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "pr", "checkout")]
public record AzReposPrCheckoutOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--remote-name")]
    public string? RemoteName { get; set; }
}