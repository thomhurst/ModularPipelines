using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-saml-provider")]
public record AwsIamUpdateSamlProviderOptions(
[property: CliOption("--saml-metadata-document")] string SamlMetadataDocument,
[property: CliOption("--saml-provider-arn")] string SamlProviderArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}