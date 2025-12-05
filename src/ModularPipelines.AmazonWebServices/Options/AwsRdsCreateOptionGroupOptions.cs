using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-option-group")]
public record AwsRdsCreateOptionGroupOptions(
[property: CliOption("--option-group-name")] string OptionGroupName,
[property: CliOption("--engine-name")] string EngineName,
[property: CliOption("--major-engine-version")] string MajorEngineVersion,
[property: CliOption("--option-group-description")] string OptionGroupDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}