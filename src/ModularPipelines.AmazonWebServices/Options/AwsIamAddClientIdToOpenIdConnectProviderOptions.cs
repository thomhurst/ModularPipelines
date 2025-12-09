using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "add-client-id-to-open-id-connect-provider")]
public record AwsIamAddClientIdToOpenIdConnectProviderOptions(
[property: CliOption("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}