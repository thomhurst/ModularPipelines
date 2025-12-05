using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "tag-open-id-connect-provider")]
public record AwsIamTagOpenIdConnectProviderOptions(
[property: CliOption("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}