using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "describe-findings")]
public record AwsInspectorDescribeFindingsOptions(
[property: CliOption("--finding-arns")] string[] FindingArns
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}