using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "change-server-life-cycle-state")]
public record AwsMgnChangeServerLifeCycleStateOptions(
[property: CliOption("--life-cycle")] string LifeCycle,
[property: CliOption("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}