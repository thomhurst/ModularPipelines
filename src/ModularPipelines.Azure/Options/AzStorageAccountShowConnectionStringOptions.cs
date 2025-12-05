using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "show-connection-string")]
public record AzStorageAccountShowConnectionStringOptions : AzOptions
{
    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--queue-endpoint")]
    public string? QueueEndpoint { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}