using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "describe-app-block-builder-app-block-associations")]
public record AwsAppstreamDescribeAppBlockBuilderAppBlockAssociationsOptions : AwsOptions
{
    [CommandSwitch("--app-block-arn")]
    public string? AppBlockArn { get; set; }

    [CommandSwitch("--app-block-builder-name")]
    public string? AppBlockBuilderName { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}