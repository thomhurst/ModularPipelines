using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "list-objects")]
public record AwsBackupstorageListObjectsOptions(
[property: CommandSwitch("--storage-job-id")] string StorageJobId
) : AwsOptions
{
    [CommandSwitch("--starting-object-name")]
    public string? StartingObjectName { get; set; }

    [CommandSwitch("--starting-object-prefix")]
    public string? StartingObjectPrefix { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--created-before")]
    public long? CreatedBefore { get; set; }

    [CommandSwitch("--created-after")]
    public long? CreatedAfter { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}