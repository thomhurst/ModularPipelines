using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-saml-provider")]
public record AwsIamUpdateSamlProviderOptions(
[property: CommandSwitch("--saml-metadata-document")] string SamlMetadataDocument,
[property: CommandSwitch("--saml-provider-arn")] string SamlProviderArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}