using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-saml-provider")]
public record AwsIamCreateSamlProviderOptions(
[property: CommandSwitch("--saml-metadata-document")] string SamlMetadataDocument,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}