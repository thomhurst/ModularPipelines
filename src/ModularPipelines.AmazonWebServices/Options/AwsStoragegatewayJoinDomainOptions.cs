using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "join-domain")]
public record AwsStoragegatewayJoinDomainOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CommandSwitch("--domain-controllers")]
    public string[]? DomainControllers { get; set; }

    [CommandSwitch("--timeout-in-seconds")]
    public int? TimeoutInSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}