using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "namespace", "client", "update")]
public record AzEventgridNamespaceClientUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--attributes")]
    public string? Attributes { get; set; }

    [CliOption("--authentication")]
    public string? Authentication { get; set; }

    [CliOption("--client-certificate-authentication")]
    public string? ClientCertificateAuthentication { get; set; }

    [CliOption("--client-name")]
    public string? ClientName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}