using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-patch-baseline-for-patch-group")]
public record AwsSsmGetPatchBaselineForPatchGroupOptions(
[property: CliOption("--patch-group")] string PatchGroup
) : AwsOptions
{
    [CliOption("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}