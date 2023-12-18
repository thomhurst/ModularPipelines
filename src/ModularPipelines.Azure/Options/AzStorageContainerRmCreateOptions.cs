using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container-rm", "create")]
public record AzStorageContainerRmCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--default-encryption-scope")]
    public string? DefaultEncryptionScope { get; set; }

    [BooleanCommandSwitch("--deny-encryption-scope-override")]
    public bool? DenyEncryptionScopeOverride { get; set; }

    [BooleanCommandSwitch("--enable-vlw")]
    public bool? EnableVlw { get; set; }

    [BooleanCommandSwitch("--fail-on-exist")]
    public bool? FailOnExist { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--public-access")]
    public string? PublicAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--root-squash")]
    public string? RootSquash { get; set; }
}

