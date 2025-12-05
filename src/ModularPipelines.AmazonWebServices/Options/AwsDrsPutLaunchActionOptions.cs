using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "put-launch-action")]
public record AwsDrsPutLaunchActionOptions(
[property: CliOption("--action-code")] string ActionCode,
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--action-version")] string ActionVersion,
[property: CliOption("--category")] string Category,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--order")] int Order,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}