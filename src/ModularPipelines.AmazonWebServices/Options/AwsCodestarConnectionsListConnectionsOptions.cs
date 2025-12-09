using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "list-connections")]
public record AwsCodestarConnectionsListConnectionsOptions : AwsOptions
{
    [CliOption("--provider-type-filter")]
    public string? ProviderTypeFilter { get; set; }

    [CliOption("--host-arn-filter")]
    public string? HostArnFilter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}