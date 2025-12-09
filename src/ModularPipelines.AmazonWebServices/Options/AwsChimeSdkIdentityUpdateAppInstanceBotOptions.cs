using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "update-app-instance-bot")]
public record AwsChimeSdkIdentityUpdateAppInstanceBotOptions(
[property: CliOption("--app-instance-bot-arn")] string AppInstanceBotArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--metadata")] string Metadata
) : AwsOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}