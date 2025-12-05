using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-service-sync-blocker")]
public record AwsProtonUpdateServiceSyncBlockerOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--resolved-reason")] string ResolvedReason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}