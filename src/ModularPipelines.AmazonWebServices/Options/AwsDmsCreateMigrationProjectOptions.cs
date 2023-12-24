using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-migration-project")]
public record AwsDmsCreateMigrationProjectOptions(
[property: CommandSwitch("--source-data-provider-descriptors")] string[] SourceDataProviderDescriptors,
[property: CommandSwitch("--target-data-provider-descriptors")] string[] TargetDataProviderDescriptors,
[property: CommandSwitch("--instance-profile-identifier")] string InstanceProfileIdentifier
) : AwsOptions
{
    [CommandSwitch("--migration-project-name")]
    public string? MigrationProjectName { get; set; }

    [CommandSwitch("--transformation-rules")]
    public string? TransformationRules { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--schema-conversion-application-attributes")]
    public string? SchemaConversionApplicationAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}