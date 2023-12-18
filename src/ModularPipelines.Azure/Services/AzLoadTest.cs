using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load")]
public class AzLoadTest
{
    public AzLoadTest(
        AzLoadTestAppComponent appComponent,
        AzLoadTestFile file,
        AzLoadTestServerMetric serverMetric,
        ICommand internalCommand
    )
    {
        AppComponent = appComponent;
        File = file;
        ServerMetric = serverMetric;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLoadTestAppComponent AppComponent { get; }

    public AzLoadTestFile File { get; }

    public AzLoadTestServerMetric ServerMetric { get; }

    public async Task<CommandResult> Create(AzLoadTestCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzLoadTestDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadFiles(AzLoadTestDownloadFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzLoadTestListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzLoadTestShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzLoadTestUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}