using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzRepos
{
    public AzRepos(
        AzReposImport import,
        AzReposPolicy policy,
        AzReposPr pr,
        AzReposRef @ref,
        ICommand internalCommand
    )
    {
        Import = import;
        Policy = policy;
        Pr = pr;
        Ref = @ref;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzReposImport Import { get; }

    public AzReposPolicy Policy { get; }

    public AzReposPr Pr { get; }

    public AzReposRef Ref { get; }

    public async Task<CommandResult> Create(AzReposCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzReposDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzReposListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzReposListOptions(), token);
    }

    public async Task<CommandResult> Show(AzReposShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzReposUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}