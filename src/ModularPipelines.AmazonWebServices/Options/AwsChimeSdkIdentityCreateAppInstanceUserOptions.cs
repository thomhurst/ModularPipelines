using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "create-app-instance-user")]
public record AwsChimeSdkIdentityCreateAppInstanceUserOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--app-instance-user-id")] string AppInstanceUserId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}