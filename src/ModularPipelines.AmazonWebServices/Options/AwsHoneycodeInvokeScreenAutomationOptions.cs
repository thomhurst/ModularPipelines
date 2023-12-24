using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("honeycode", "invoke-screen-automation")]
public record AwsHoneycodeInvokeScreenAutomationOptions(
[property: CommandSwitch("--workbook-id")] string WorkbookId,
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--screen-id")] string ScreenId,
[property: CommandSwitch("--screen-automation-id")] string ScreenAutomationId
) : AwsOptions
{
    [CommandSwitch("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CommandSwitch("--row-id")]
    public string? RowId { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}