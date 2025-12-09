using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-variable")]
public record AwsFrauddetectorCreateVariableOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--data-type")] string DataType,
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--default-value")] string DefaultValue
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--variable-type")]
    public string? VariableType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}