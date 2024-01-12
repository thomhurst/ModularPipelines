using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetScopes
{
    public GcloudContainerFleetScopes(
        GcloudContainerFleetScopesNamespaces namespaces,
        GcloudContainerFleetScopesRbacrolebindings rbacrolebindings,
        ICommand internalCommand
    )
    {
        Namespaces = namespaces;
        Rbacrolebindings = rbacrolebindings;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerFleetScopesNamespaces Namespaces { get; }

    public GcloudContainerFleetScopesRbacrolebindings Rbacrolebindings { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudContainerFleetScopesAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudContainerFleetScopesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudContainerFleetScopesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerFleetScopesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudContainerFleetScopesGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudContainerFleetScopesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetScopesListOptions(), token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudContainerFleetScopesRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetScopesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}