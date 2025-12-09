using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "add-option-to-option-group")]
public record AwsRdsAddOptionToOptionGroupOptions(
[property: CliOption("--option-group-name")] string OptionGroupName
) : AwsOptions
{
    [CliOption("--options")]
    public string[]? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}