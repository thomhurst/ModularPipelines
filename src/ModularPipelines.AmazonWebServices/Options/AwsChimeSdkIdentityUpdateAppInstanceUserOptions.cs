using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "update-app-instance-user")]
public record AwsChimeSdkIdentityUpdateAppInstanceUserOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--metadata")] string Metadata
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}