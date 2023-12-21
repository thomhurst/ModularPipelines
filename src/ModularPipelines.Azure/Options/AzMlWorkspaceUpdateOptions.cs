using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace", "update")]
public record AzMlWorkspaceUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--allow-public-access")]
    public bool? AllowPublicAccess { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--image-build-compute")]
    public string? ImageBuildCompute { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--v1-legacy-mode")]
    public string? V1LegacyMode { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}