using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "test-state")]
public record AwsStepfunctionsTestStateOptions(
[property: CliOption("--definition")] string Definition,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--inspection-level")]
    public string? InspectionLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}