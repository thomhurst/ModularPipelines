using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-default-patch-baseline")]
public record AwsSsmGetDefaultPatchBaselineOptions : AwsOptions
{
    [CliOption("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}