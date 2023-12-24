using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-service-last-accessed-details-with-entities")]
public record AwsIamGetServiceLastAccessedDetailsWithEntitiesOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--service-namespace")] string ServiceNamespace
) : AwsOptions
{
    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}