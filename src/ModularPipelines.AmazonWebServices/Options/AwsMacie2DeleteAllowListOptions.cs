using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "delete-allow-list")]
public record AwsMacie2DeleteAllowListOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--ignore-job-checks")]
    public string? IgnoreJobChecks { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}