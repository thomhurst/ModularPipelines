using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-lifecycle-policy")]
public record AwsImagebuilderCreateLifecyclePolicyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--execution-role")] string ExecutionRole,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--policy-details")] string[] PolicyDetails,
[property: CliOption("--resource-selection")] string ResourceSelection
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}