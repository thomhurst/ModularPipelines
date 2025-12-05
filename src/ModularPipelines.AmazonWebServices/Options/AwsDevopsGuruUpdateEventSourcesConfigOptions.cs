using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "update-event-sources-config")]
public record AwsDevopsGuruUpdateEventSourcesConfigOptions : AwsOptions
{
    [CliOption("--event-sources")]
    public string? EventSources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}