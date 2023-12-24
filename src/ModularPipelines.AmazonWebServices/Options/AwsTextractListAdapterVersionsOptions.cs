using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "list-adapter-versions")]
public record AwsTextractListAdapterVersionsOptions : AwsOptions
{
    [CommandSwitch("--adapter-id")]
    public string? AdapterId { get; set; }

    [CommandSwitch("--after-creation-time")]
    public long? AfterCreationTime { get; set; }

    [CommandSwitch("--before-creation-time")]
    public long? BeforeCreationTime { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}