using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "client", "create")]
public record AzEventgridNamespaceClientCreateOptions(
[property: CliOption("--client-name")] string ClientName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--attributes")]
    public string? Attributes { get; set; }

    [CliOption("--authentication")]
    public string? Authentication { get; set; }

    [CliOption("--authentication-name")]
    public string? AuthenticationName { get; set; }

    [CliOption("--client-certificate-authentication")]
    public string? ClientCertificateAuthentication { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}