using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "list-apps")]
public record AwsResiliencehubListAppsOptions : AwsOptions
{
    [CommandSwitch("--app-arn")]
    public string? AppArn { get; set; }

    [CommandSwitch("--from-last-assessment-time")]
    public long? FromLastAssessmentTime { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--to-last-assessment-time")]
    public long? ToLastAssessmentTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}