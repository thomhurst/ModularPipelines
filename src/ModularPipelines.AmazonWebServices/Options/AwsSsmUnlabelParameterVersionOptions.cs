using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "unlabel-parameter-version")]
public record AwsSsmUnlabelParameterVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--parameter-version")] long ParameterVersion,
[property: CliOption("--labels")] string[] Labels
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}