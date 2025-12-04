using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "data-connector", "connect")]
public record AzSentinelDataConnectorConnectOptions(
[property: CliOption("--data-connector-id")] string DataConnectorId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--authorization-code")]
    public string? AuthorizationCode { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--output-stream")]
    public string? OutputStream { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--rule-immutable-id")]
    public string? RuleImmutableId { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }
}