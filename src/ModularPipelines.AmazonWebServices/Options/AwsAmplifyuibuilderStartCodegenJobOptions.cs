using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "start-codegen-job")]
public record AwsAmplifyuibuilderStartCodegenJobOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--codegen-job-to-create")] string CodegenJobToCreate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}