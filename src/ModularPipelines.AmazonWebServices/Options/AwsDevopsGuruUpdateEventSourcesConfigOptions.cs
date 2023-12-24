using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "update-event-sources-config")]
public record AwsDevopsGuruUpdateEventSourcesConfigOptions : AwsOptions
{
    [CommandSwitch("--event-sources")]
    public string? EventSources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}