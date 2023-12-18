using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "message", "update")]
public record AzStorageMessageUpdateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--pop-receipt")] string PopReceipt,
[property: CommandSwitch("--queue-name")] string QueueName
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--queue-endpoint")]
    public string? QueueEndpoint { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--visibility-timeout")]
    public string? VisibilityTimeout { get; set; }
}