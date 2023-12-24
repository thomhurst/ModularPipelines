using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "create-component")]
public record AwsAmplifyuibuilderCreateComponentOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--component-to-create")] string ComponentToCreate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}