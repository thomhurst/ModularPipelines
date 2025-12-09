using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-open-id-connect-provider")]
public record AwsIamDeleteOpenIdConnectProviderOptions(
[property: CliOption("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}