using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-saml-properties")]
public record AwsWorkspacesModifySamlPropertiesOptions(
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--saml-properties")]
    public string? SamlProperties { get; set; }

    [CommandSwitch("--properties-to-delete")]
    public string[]? PropertiesToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}