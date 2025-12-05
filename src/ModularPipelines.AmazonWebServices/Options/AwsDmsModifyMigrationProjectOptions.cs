using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-migration-project")]
public record AwsDmsModifyMigrationProjectOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier
) : AwsOptions
{
    [CliOption("--migration-project-name")]
    public string? MigrationProjectName { get; set; }

    [CliOption("--source-data-provider-descriptors")]
    public string[]? SourceDataProviderDescriptors { get; set; }

    [CliOption("--target-data-provider-descriptors")]
    public string[]? TargetDataProviderDescriptors { get; set; }

    [CliOption("--instance-profile-identifier")]
    public string? InstanceProfileIdentifier { get; set; }

    [CliOption("--transformation-rules")]
    public string? TransformationRules { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schema-conversion-application-attributes")]
    public string? SchemaConversionApplicationAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}