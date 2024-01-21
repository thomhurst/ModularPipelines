using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Services;

[ExcludeFromCodeCoverage]
public class DotNetNuget
{
    public DotNetNuget(
        DotNetNugetDisable disable,
        DotNetNugetRemove remove,
        DotNetNugetEnable enable,
        DotNetNugetUpdate update,
        DotNetNugetAdd add,
        DotNetNugetList list,
        ICommand internalCommand
    )
    {
        Disable = disable;
        Remove = remove;
        Enable = enable;
        Update = update;
        Add = add;
        List = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DotNetNugetDisable Disable { get; }

    public DotNetNugetRemove Remove { get; }

    public DotNetNugetEnable Enable { get; }

    public DotNetNugetUpdate Update { get; }

    public DotNetNugetAdd Add { get; }

    public DotNetNugetList List { get; }

    public async Task<CommandResult> Delete(DotNetNugetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetNugetDeleteOptions(), token);
    }

    public async Task<CommandResult> Locals(DotNetNugetLocalsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Push(DotNetNugetPushOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetNugetPushOptions(), token);
    }

    public async Task<CommandResult> Verify(DotNetNugetVerifyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetNugetVerifyOptions(), token);
    }

    public async Task<CommandResult> Trust(DotNetNugetTrustOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetNugetTrustOptions(), token);
    }

    public async Task<CommandResult> Sign(DotNetNugetSignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetNugetSignOptions(), token);
    }
}
