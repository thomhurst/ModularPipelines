using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "list-replace-permission-associations-work")]
public record AwsRamListReplacePermissionAssociationsWorkOptions : AwsOptions
{
    [CliOption("--work-ids")]
    public string[]? WorkIds { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}