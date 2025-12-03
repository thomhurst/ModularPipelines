using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubScopes
{
    public GcloudContainerHubScopes(
        GcloudContainerHubScopesNamespaces namespaces,
        GcloudContainerHubScopesRbacrolebindings rbacrolebindings,
        ICommand internalCommand
    )
    {
        Namespaces = namespaces;
        Rbacrolebindings = rbacrolebindings;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerHubScopesNamespaces Namespaces { get; }

    public GcloudContainerHubScopesRbacrolebindings Rbacrolebindings { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudContainerHubScopesAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudContainerHubScopesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudContainerHubScopesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubScopesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudContainerHubScopesGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudContainerHubScopesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubScopesListOptions(), token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudContainerHubScopesRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubScopesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}