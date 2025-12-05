using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-migration-project")]
public record AwsDmsCreateMigrationProjectOptions(
[property: CliOption("--source-data-provider-descriptors")] string[] SourceDataProviderDescriptors,
[property: CliOption("--target-data-provider-descriptors")] string[] TargetDataProviderDescriptors,
[property: CliOption("--instance-profile-identifier")] string InstanceProfileIdentifier
) : AwsOptions
{
    [CliOption("--migration-project-name")]
    public string? MigrationProjectName { get; set; }

    [CliOption("--transformation-rules")]
    public string? TransformationRules { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--schema-conversion-application-attributes")]
    public string? SchemaConversionApplicationAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}