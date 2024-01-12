using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy-troubleshoot", "iam")]
public record GcloudPolicyTroubleshootIamOptions(
[property: PositionalArgument] string Resource,
[property: CommandSwitch("--permission")] string Permission,
[property: CommandSwitch("--principal-email")] string PrincipalEmail
) : GcloudOptions
{
    [CommandSwitch("--destination-ip")]
    public string? DestinationIp { get; set; }

    [CommandSwitch("--destination-port")]
    public string? DestinationPort { get; set; }

    [CommandSwitch("--request-time")]
    public string? RequestTime { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--resource-service")]
    public string? ResourceService { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}