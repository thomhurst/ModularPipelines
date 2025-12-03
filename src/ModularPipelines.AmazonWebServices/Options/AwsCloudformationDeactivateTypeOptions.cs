using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "deactivate-type")]
public record AwsCloudformationDeactivateTypeOptions : AwsOptions
{
    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}