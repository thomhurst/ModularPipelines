using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-key-signing-key")]
public record AwsRoute53CreateKeySigningKeyOptions(
[property: CommandSwitch("--caller-reference")] string CallerReference,
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--key-management-service-arn")] string KeyManagementServiceArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}