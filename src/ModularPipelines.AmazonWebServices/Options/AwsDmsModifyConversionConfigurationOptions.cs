using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-conversion-configuration")]
public record AwsDmsModifyConversionConfigurationOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CliOption("--conversion-configuration")] string ConversionConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}