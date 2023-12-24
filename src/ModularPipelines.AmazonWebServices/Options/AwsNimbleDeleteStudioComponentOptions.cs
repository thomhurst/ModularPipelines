using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "delete-studio-component")]
public record AwsNimbleDeleteStudioComponentOptions(
[property: CommandSwitch("--studio-component-id")] string StudioComponentId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}