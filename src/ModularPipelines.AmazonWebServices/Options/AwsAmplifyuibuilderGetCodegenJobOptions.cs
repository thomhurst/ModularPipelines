using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "get-codegen-job")]
public record AwsAmplifyuibuilderGetCodegenJobOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}