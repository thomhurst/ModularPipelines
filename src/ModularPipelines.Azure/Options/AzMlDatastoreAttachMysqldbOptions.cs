using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "datastore", "attach-mysqldb")]
public record AzMlDatastoreAttachMysqldbOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--password")] string Password,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--user-id")] string UserId
) : AzOptions
{
    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}