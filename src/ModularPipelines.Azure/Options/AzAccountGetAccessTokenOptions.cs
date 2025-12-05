using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "get-access-token")]
public record AzAccountGetAccessTokenOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--tenant")]
    public string? Tenant { get; set; }
}