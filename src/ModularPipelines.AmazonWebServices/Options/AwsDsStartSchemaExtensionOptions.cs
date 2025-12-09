using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "start-schema-extension")]
public record AwsDsStartSchemaExtensionOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--ldif-content")] string LdifContent,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}