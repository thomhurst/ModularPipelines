using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-sensitivity-inspection-template")]
public record AwsMacie2UpdateSensitivityInspectionTemplateOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--excludes")]
    public string? Excludes { get; set; }

    [CliOption("--includes")]
    public string? Includes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}