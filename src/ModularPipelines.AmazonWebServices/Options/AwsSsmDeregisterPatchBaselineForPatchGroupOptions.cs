using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "deregister-patch-baseline-for-patch-group")]
public record AwsSsmDeregisterPatchBaselineForPatchGroupOptions(
[property: CliOption("--baseline-id")] string BaselineId,
[property: CliOption("--patch-group")] string PatchGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}