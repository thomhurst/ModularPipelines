using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("provider", "register")]
public record AzProviderRegisterOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions
{
    [CliFlag("--consent-to-permissions")]
    public bool? ConsentToPermissions { get; set; }

    [CliOption("--management-group-id")]
    public string? ManagementGroupId { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }
}