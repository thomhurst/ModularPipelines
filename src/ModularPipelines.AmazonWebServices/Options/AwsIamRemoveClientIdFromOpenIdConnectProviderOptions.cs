using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "remove-client-id-from-open-id-connect-provider")]
public record AwsIamRemoveClientIdFromOpenIdConnectProviderOptions(
[property: CliOption("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}