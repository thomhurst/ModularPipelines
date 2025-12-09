using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-launch-template-versions")]
public record AwsEc2DescribeLaunchTemplateVersionsOptions : AwsOptions
{
    [CliOption("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CliOption("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CliOption("--versions")]
    public string[]? Versions { get; set; }

    [CliOption("--min-version")]
    public string? MinVersion { get; set; }

    [CliOption("--max-version")]
    public string? MaxVersion { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}