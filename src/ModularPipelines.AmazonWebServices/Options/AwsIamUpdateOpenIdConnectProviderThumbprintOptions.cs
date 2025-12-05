using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-open-id-connect-provider-thumbprint")]
public record AwsIamUpdateOpenIdConnectProviderThumbprintOptions(
[property: CliOption("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CliOption("--thumbprint-list")] string[] ThumbprintList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}