using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-template-sync-config")]
public record AwsProtonCreateTemplateSyncConfigOptions(
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--repository-provider")] string RepositoryProvider,
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-type")] string TemplateType
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}