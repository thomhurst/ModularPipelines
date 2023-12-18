using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "namespace", "client", "create")]
public record AzEventgridNamespaceClientCreateOptions(
[property: CommandSwitch("--client-name")] string ClientName,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--attributes")]
    public string? Attributes { get; set; }

    [CommandSwitch("--authentication")]
    public string? Authentication { get; set; }

    [CommandSwitch("--authentication-name")]
    public string? AuthenticationName { get; set; }

    [CommandSwitch("--client-certificate-authentication")]
    public string? ClientCertificateAuthentication { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}