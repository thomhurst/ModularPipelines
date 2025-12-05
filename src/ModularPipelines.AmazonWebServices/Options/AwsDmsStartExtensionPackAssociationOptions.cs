using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-extension-pack-association")]
public record AwsDmsStartExtensionPackAssociationOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}