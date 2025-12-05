using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-macie-session")]
public record AwsMacie2UpdateMacieSessionOptions : AwsOptions
{
    [CliOption("--finding-publishing-frequency")]
    public string? FindingPublishingFrequency { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}