using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitVersioning : IGitVersioning
{
    private readonly ICommand _command;
    private readonly IEnvironmentContext _environmentContext;
    
    private readonly Folder _temporaryFolder;

    private GitVersionInformation? _prefetchedGitVersionInformation;

    public GitVersioning(IFileSystemContext fileSystemContext, ICommand command, IEnvironmentContext environmentContext)
    {
        _command = command;
        _environmentContext = environmentContext;
        _temporaryFolder = fileSystemContext.CreateTemporaryFolder();
    }
    
    public async Task<GitVersionInformation> GetGitVersioningInformation()
    {
        if (_prefetchedGitVersionInformation != null)
        {
            return _prefetchedGitVersionInformation;
        }
        
        await _command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = new[]
            {
                "tool", "install", "--tool-path", _temporaryFolder.Path, "GitVersion.Tool"
            }
        });
        
        var gitVersionOutput = await _command.ExecuteCommandLineTool(new CommandLineToolOptions(Path.Combine(_temporaryFolder, "dotnet-gitversion"))
        {
            WorkingDirectory = _environmentContext.GitRootDirectory!.Path,
            Arguments = new[]
            {
                "/output", "json"
            }
        });

        return _prefetchedGitVersionInformation ??= JsonSerializer.Deserialize<GitVersionInformation>(gitVersionOutput.StandardOutput)!;
    }
}