using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "enable-federation")]
public record AwsCloudtrailEnableFederationOptions(
[property: CommandSwitch("--event-data-store")] string EventDataStore,
[property: CommandSwitch("--federation-role-arn")] string FederationRoleArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}