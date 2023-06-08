using ModularPipelines.Context;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Git;

public class GitInformation<T> : IGitInformation<T>, IInitializer
{
    private readonly IModuleContext<T> _context;

    public GitInformation(IModuleContext<T> context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await RunCommands("version");
        }
        catch (Exception e)
        {
            throw new Exception("Error detecting Git repository", e);
        }
        
        BranchName = await RunCommands("rev-parse", "--abbrev-ref", "HEAD");
        DefaultBranchName = await RunCommands("rev-parse", "--abbrev-ref", "origin/HEAD");
        LastCommitSha = await RunCommands("rev-parse", "HEAD");
        LastCommitShortSha = await  RunCommands("rev-parse", "--short", "HEAD");
        Tag =  await RunCommands("describe", "--tags");
        CommitsOnBranch =  int.Parse(await RunCommands("rev-list", "HEAD", "--count"));
        LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(await RunCommands("log", "-1", "--format=%at")));
    }

    public string BranchName { get; private set; }
    public string DefaultBranchName { get; private set; }

    public string Tag { get; private set; }

    public string LastCommit { get; private set; }

    public async Task<IEnumerable<string>> LastCommits(int count)
    {
        var result = await RunCommands("log", $"-{count}");
        return result.Split(Environment.NewLine);
    }

    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
        
    public string LastCommitSha { get; private set; }

    public string LastCommitShortSha { get; private set; }

    private async Task<string> RunCommands(params string[] commands)
    {
        var commandResult = await _context.Command.UsingCommandLineTool(
            new CommandLineToolOptions("git")
            {
                Arguments = commands
            });

        return commandResult.StandardOutput.Trim();
    }
}