using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "list-adapters")]
public record AwsTextractListAdaptersOptions : AwsOptions
{
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