using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf")]
public class AzSfCluster
{
    public AzSfCluster(
        AzSfClusterClientCertificate clientCertificate,
        AzSfClusterDurability durability,
        AzSfClusterNode node,
        AzSfClusterNodeType nodeType,
        AzSfClusterReliability reliability,
        AzSfClusterSetting setting,
        AzSfClusterUpgradeType upgradeType,
        ICommand internalCommand
    )
    {
        ClientCertificate = clientCertificate;
        Durability = durability;
        Node = node;
        NodeType = nodeType;
        Reliability = reliability;
        Setting = setting;
        UpgradeType = upgradeType;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSfClusterClientCertificate ClientCertificate { get; }

    public AzSfClusterDurability Durability { get; }

    public AzSfClusterNode Node { get; }

    public AzSfClusterNodeType NodeType { get; }

    public AzSfClusterReliability Reliability { get; }

    public AzSfClusterSetting Setting { get; }

    public AzSfClusterUpgradeType UpgradeType { get; }

    public async Task<CommandResult> Create(AzSfClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSfClusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSfClusterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}