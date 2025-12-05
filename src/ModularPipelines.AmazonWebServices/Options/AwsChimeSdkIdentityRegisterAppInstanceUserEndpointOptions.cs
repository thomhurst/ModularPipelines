using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "register-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityRegisterAppInstanceUserEndpointOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CliOption("--type")] string Type,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--endpoint-attributes")] string EndpointAttributes
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--allow-messages")]
    public string? AllowMessages { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}