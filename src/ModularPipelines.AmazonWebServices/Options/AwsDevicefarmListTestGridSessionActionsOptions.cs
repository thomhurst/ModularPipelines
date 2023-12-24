using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "list-test-grid-session-actions")]
public record AwsDevicefarmListTestGridSessionActionsOptions(
[property: CommandSwitch("--session-arn")] string SessionArn
) : AwsOptions
{
    [CommandSwitch("--max-result")]
    public int? MaxResult { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}