using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "create-app-instance-bot")]
public record AwsChimeSdkIdentityCreateAppInstanceBotOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}