using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "update-lifecycle-policy")]
public record AwsImagebuilderUpdateLifecyclePolicyOptions(
[property: CliOption("--lifecycle-policy-arn")] string LifecyclePolicyArn,
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

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}