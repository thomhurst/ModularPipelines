using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "list-connections")]
public record AwsCodestarConnectionsListConnectionsOptions : AwsOptions
{
    [CommandSwitch("--provider-type-filter")]
    public string? ProviderTypeFilter { get; set; }

    [CommandSwitch("--host-arn-filter")]
    public string? HostArnFilter { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}