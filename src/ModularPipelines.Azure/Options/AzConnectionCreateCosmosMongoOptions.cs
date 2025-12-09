using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connection", "create", "cosmos-mongo")]
public record AzConnectionCreateCosmosMongoOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliOption("--target-id")]
    public string? TargetId { get; set; }

    [CliOption("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CliOption("--user-account")]
    public int? UserAccount { get; set; }
}