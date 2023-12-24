using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "run-statement")]
public record AwsGlueRunStatementOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--code")] string Code
) : AwsOptions
{
    [CommandSwitch("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}