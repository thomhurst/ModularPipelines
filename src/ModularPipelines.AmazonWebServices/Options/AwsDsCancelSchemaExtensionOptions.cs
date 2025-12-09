using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "cancel-schema-extension")]
public record AwsDsCancelSchemaExtensionOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--schema-extension-id")] string SchemaExtensionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}