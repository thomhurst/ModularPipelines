using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "invoke-screen-automation")]
public record AwsHoneycodeInvokeScreenAutomationOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--screen-id")] string ScreenId,
[property: CliOption("--screen-automation-id")] string ScreenAutomationId
) : AwsOptions
{
    [CliOption("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CliOption("--row-id")]
    public string? RowId { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}