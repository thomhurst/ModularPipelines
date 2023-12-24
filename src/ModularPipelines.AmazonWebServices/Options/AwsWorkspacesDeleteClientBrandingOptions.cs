using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "delete-client-branding")]
public record AwsWorkspacesDeleteClientBrandingOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--platforms")] string[] Platforms
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}