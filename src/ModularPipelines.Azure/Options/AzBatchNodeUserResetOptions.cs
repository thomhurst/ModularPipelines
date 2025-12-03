using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "node", "user", "reset")]
public record AzBatchNodeUserResetOptions(
[property: CliOption("--node-id")] string NodeId,
[property: CliOption("--pool-id")] string PoolId,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--expiry-time")]
    public string? ExpiryTime { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }
}