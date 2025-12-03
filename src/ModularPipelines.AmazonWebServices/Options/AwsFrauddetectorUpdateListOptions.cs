using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-list")]
public record AwsFrauddetectorUpdateListOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--elements")]
    public string[]? Elements { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--update-mode")]
    public string? UpdateMode { get; set; }

    [CliOption("--variable-type")]
    public string? VariableType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}