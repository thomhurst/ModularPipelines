using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-saml-provider")]
public record AwsIamCreateSamlProviderOptions(
[property: CliOption("--saml-metadata-document")] string SamlMetadataDocument,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}