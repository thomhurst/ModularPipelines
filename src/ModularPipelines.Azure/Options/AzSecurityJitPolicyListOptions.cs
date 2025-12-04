using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "jit-policy", "list")]
public record AzSecurityJitPolicyListOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}