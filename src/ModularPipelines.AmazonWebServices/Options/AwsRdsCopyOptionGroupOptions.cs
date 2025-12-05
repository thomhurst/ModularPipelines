using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "copy-option-group")]
public record AwsRdsCopyOptionGroupOptions(
[property: CliOption("--source-option-group-identifier")] string SourceOptionGroupIdentifier,
[property: CliOption("--target-option-group-identifier")] string TargetOptionGroupIdentifier,
[property: CliOption("--target-option-group-description")] string TargetOptionGroupDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}