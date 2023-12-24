using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-migration-project")]
public record AwsDmsModifyMigrationProjectOptions(
[property: CommandSwitch("--migration-project-identifier")] string MigrationProjectIdentifier
) : AwsOptions
{
    [CommandSwitch("--migration-project-name")]
    public string? MigrationProjectName { get; set; }

    [CommandSwitch("--source-data-provider-descriptors")]
    public string[]? SourceDataProviderDescriptors { get; set; }

    [CommandSwitch("--target-data-provider-descriptors")]
    public string[]? TargetDataProviderDescriptors { get; set; }

    [CommandSwitch("--instance-profile-identifier")]
    public string? InstanceProfileIdentifier { get; set; }

    [CommandSwitch("--transformation-rules")]
    public string? TransformationRules { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schema-conversion-application-attributes")]
    public string? SchemaConversionApplicationAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}