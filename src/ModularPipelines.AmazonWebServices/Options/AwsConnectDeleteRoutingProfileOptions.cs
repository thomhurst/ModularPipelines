using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-routing-profile")]
public record AwsConnectDeleteRoutingProfileOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}