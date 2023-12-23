using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-cases")]
public record AwsSupportDescribeCasesOptions : AwsOptions
{
    [CommandSwitch("--case-id-list")]
    public string[]? CaseIdList { get; set; }

    [CommandSwitch("--display-id")]
    public string? DisplayId { get; set; }

    [CommandSwitch("--after-time")]
    public string? AfterTime { get; set; }

    [CommandSwitch("--before-time")]
    public string? BeforeTime { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}