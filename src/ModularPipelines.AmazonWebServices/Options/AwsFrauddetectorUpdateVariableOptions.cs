using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-variable")]
public record AwsFrauddetectorUpdateVariableOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--default-value")]
    public string? DefaultValue { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--variable-type")]
    public string? VariableType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}