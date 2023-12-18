using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridNamespace
{
    public AzEventgridNamespace(
        AzEventgridNamespaceCaCertificate caCertificate,
        AzEventgridNamespaceClient client,
        AzEventgridNamespaceClientGroup clientGroup,
        AzEventgridNamespacePermissionBinding permissionBinding,
        AzEventgridNamespaceTopic topic,
        AzEventgridNamespaceTopicSpace topicSpace,
        ICommand internalCommand
    )
    {
        CaCertificate = caCertificate;
        Client = client;
        ClientGroup = clientGroup;
        PermissionBinding = permissionBinding;
        Topic = topic;
        TopicSpace = topicSpace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridNamespaceCaCertificate CaCertificate { get; }

    public AzEventgridNamespaceClient Client { get; }

    public AzEventgridNamespaceClientGroup ClientGroup { get; }

    public AzEventgridNamespacePermissionBinding PermissionBinding { get; }

    public AzEventgridNamespaceTopic Topic { get; }

    public AzEventgridNamespaceTopicSpace TopicSpace { get; }

    public async Task<CommandResult> Create(AzEventgridNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridNamespaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceListOptions(), token);
    }

    public async Task<CommandResult> ListKey(AzEventgridNamespaceListKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceListKeyOptions(), token);
    }

    public async Task<CommandResult> RegenerateKey(AzEventgridNamespaceRegenerateKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridNamespaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventgridNamespaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceWaitOptions(), token);
    }
}