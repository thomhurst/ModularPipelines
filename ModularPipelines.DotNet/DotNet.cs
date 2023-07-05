using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;

namespace ModularPipelines.DotNet;

public class DotNet : IDotNet
{
    private readonly ICommand _command;
    private readonly IFileSystemContext _fileSystemContext;
    private readonly ITrxParser _trxParser;

    public DotNet( ITrxParser trxParser, ICommand command, IFileSystemContext fileSystemContext )
    {
        _trxParser = trxParser;
        _command = command;
        _fileSystemContext = fileSystemContext;
    }

    public Task<CommandResult> Restore( DotNetRestoreOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "restore" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public Task<CommandResult> Build( DotNetBuildOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "build" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public Task<CommandResult> Publish( DotNetPublishOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "publish" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public Task<CommandResult> Pack( DotNetPackOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "pack" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public Task<CommandResult> Clean( DotNetCleanOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "clean" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public async Task<DotNetTestResult> Test( DotNetTestOptions options, CancellationToken cancellationToken = default )
    {
        var trxFilePath = Path.GetTempFileName();

        options.Logger ??= new List<string>();
        options.Logger.Add( $"trx;logfilename={trxFilePath}" );

        var args = new List<string> { "test" };
        args.AddNonNullOrEmpty( options.TargetPath );
        await ExecuteCommandLineTool( options, args, cancellationToken );

        var trxContents = await _fileSystemContext.GetFile( trxFilePath ).ReadAsync();

        return _trxParser.ParseTestResult( trxContents );
    }

    public Task<CommandResult> Format( DotNetFormatOptions options, CancellationToken cancellationToken = default )
    {
        var args = new List<string> { "format" };
        args.AddNonNullOrEmpty( options.TargetPath );
        return ExecuteCommandLineTool( options, args, cancellationToken );
    }

    public Task<CommandResult> Version( DotNetOptions? options, CancellationToken cancellationToken = default )
    {
        options ??= new DotNetOptions();

        return ExecuteCommandLineTool( options, new[] { "--version" }, cancellationToken );
    }

    public Task<CommandResult> CustomCommand( DotNetCommandOptions options, CancellationToken cancellationToken = default )
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions( "dotnet", options.Command ?? ArraySegment<string>.Empty ) with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool( commandLineToolOptions, cancellationToken );
    }

    private Task<CommandResult> ExecuteCommandLineTool( DotNetOptions options, IEnumerable<string> arguments, CancellationToken cancellationToken )
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions( "dotnet", arguments ) with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool( commandLineToolOptions, cancellationToken );
    }
}
