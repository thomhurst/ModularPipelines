using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-organizations-access-report")]
public record AwsIamGetOrganizationsAccessReportOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--sort-key")]
    public string? SortKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}