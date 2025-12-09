using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "label-parameter-version")]
public record AwsSsmLabelParameterVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--labels")] string[] Labels
) : AwsOptions
{
    [CliOption("--parameter-version")]
    public long? ParameterVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}