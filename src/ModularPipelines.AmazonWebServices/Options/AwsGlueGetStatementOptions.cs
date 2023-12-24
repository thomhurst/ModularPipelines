using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-statement")]
public record AwsGlueGetStatementOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--id")] int Id
) : AwsOptions
{
    [CommandSwitch("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}