using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "test-authorization")]
public record AwsIotTestAuthorizationOptions(
[property: CliOption("--auth-infos")] string[] AuthInfos
) : AwsOptions
{
    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--cognito-identity-pool-id")]
    public string? CognitoIdentityPoolId { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--policy-names-to-add")]
    public string[]? PolicyNamesToAdd { get; set; }

    [CliOption("--policy-names-to-skip")]
    public string[]? PolicyNamesToSkip { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}