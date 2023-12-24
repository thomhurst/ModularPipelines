using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("applicationcostprofiler", "import-application-usage")]
public record AwsApplicationcostprofilerImportApplicationUsageOptions(
[property: CommandSwitch("--source-s3-location")] string SourceS3Location
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}