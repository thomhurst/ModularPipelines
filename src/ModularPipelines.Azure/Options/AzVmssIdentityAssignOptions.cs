using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "identity", "assign")]
public record AzVmssIdentityAssignOptions : AzOptions
{
    [CliOption("--identities")]
    public string? Identities { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}