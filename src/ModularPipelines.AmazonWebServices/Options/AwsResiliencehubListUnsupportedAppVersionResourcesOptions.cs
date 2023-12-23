using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "list-unsupported-app-version-resources")]
public record AwsResiliencehubListUnsupportedAppVersionResourcesOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-version")] string AppVersion
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--resolution-id")]
    public string? ResolutionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}