using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "snapshot-completed")]
public record AwsEc2WaitSnapshotCompletedOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--owner-ids")]
    public string[]? OwnerIds { get; set; }

    [CliOption("--restorable-by-user-ids")]
    public string[]? RestorableByUserIds { get; set; }

    [CliOption("--snapshot-ids")]
    public string[]? SnapshotIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}