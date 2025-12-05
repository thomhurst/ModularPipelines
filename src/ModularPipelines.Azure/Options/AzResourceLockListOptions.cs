using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "lock", "list")]
public record AzResourceLockListOptions : AzOptions
{
    [CliOption("--filter-string")]
    public string? FilterString { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}