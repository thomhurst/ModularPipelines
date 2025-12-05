using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "message", "put")]
public record AzStorageMessagePutOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--queue-name")] string QueueName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--queue-endpoint")]
    public string? QueueEndpoint { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--time-to-live")]
    public string? TimeToLive { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--visibility-timeout")]
    public string? VisibilityTimeout { get; set; }
}