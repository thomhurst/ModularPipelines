using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "get-temporary-glue-partition-credentials")]
public record AwsLakeformationGetTemporaryGluePartitionCredentialsOptions(
[property: CommandSwitch("--table-arn")] string TableArn,
[property: CommandSwitch("--partition")] string Partition
) : AwsOptions
{
    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--audit-context")]
    public string? AuditContext { get; set; }

    [CommandSwitch("--supported-permission-types")]
    public string[]? SupportedPermissionTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}