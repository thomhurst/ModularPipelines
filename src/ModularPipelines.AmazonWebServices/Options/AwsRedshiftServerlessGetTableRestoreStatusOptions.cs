using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "get-table-restore-status")]
public record AwsRedshiftServerlessGetTableRestoreStatusOptions(
[property: CommandSwitch("--table-restore-request-id")] string TableRestoreRequestId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}