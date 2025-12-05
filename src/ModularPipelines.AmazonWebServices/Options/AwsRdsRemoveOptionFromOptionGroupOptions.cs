using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "remove-option-from-option-group")]
public record AwsRdsRemoveOptionFromOptionGroupOptions(
[property: CliOption("--option-group-name")] string OptionGroupName
) : AwsOptions
{
    [CliOption("--options")]
    public string[]? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}