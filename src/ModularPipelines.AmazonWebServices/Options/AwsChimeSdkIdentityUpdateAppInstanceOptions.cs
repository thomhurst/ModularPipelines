using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "update-app-instance")]
public record AwsChimeSdkIdentityUpdateAppInstanceOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--metadata")] string Metadata
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}