using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-service-sync-blocker")]
public record AwsProtonUpdateServiceSyncBlockerOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--resolved-reason")] string ResolvedReason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}