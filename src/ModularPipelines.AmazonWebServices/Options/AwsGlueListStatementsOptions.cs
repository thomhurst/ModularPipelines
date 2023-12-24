using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "list-statements")]
public record AwsGlueListStatementsOptions(
[property: CommandSwitch("--session-id")] string SessionId
) : AwsOptions
{
    [CommandSwitch("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}