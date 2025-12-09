using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("quantum", "workspace", "create")]
public record AzQuantumWorkspaceCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--auto-accept")]
    public bool? AutoAccept { get; set; }

    [CliOption("--provider-sku-list")]
    public string? ProviderSkuList { get; set; }

    [CliFlag("--skip-autoadd")]
    public bool? SkipAutoadd { get; set; }

    [CliFlag("--skip-role-assignment")]
    public bool? SkipRoleAssignment { get; set; }
}