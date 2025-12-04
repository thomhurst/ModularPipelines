using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "queue", "policy", "update")]
public record AzStorageQueuePolicyUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--queue-name")] string QueueName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--expiry")]
    public string? Expiry { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--queue-endpoint")]
    public string? QueueEndpoint { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }
}