using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "delete-policy")]
public record AzKeyvaultDeletePolicyOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--application-id")]
    public string? ApplicationId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--spn")]
    public string? Spn { get; set; }

    [CliOption("--upn")]
    public string? Upn { get; set; }
}