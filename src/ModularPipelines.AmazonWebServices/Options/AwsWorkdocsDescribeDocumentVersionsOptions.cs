using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "describe-document-versions")]
public record AwsWorkdocsDescribeDocumentVersionsOptions(
[property: CommandSwitch("--document-id")] string DocumentId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--fields")]
    public string? Fields { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}