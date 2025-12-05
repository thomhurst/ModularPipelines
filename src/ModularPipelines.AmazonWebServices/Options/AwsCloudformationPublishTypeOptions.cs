using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "publish-type")]
public record AwsCloudformationPublishTypeOptions : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--public-version-number")]
    public string? PublicVersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}