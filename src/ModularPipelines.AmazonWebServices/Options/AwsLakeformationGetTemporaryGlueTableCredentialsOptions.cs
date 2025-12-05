using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "get-temporary-glue-table-credentials")]
public record AwsLakeformationGetTemporaryGlueTableCredentialsOptions(
[property: CliOption("--table-arn")] string TableArn
) : AwsOptions
{
    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--audit-context")]
    public string? AuditContext { get; set; }

    [CliOption("--supported-permission-types")]
    public string[]? SupportedPermissionTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}