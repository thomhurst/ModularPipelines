using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-conversion-configuration")]
public record AwsDmsModifyConversionConfigurationOptions(
[property: CommandSwitch("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CommandSwitch("--conversion-configuration")] string ConversionConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}