using ModularPipelines.Context;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Git;

internal class StaticGitInformation : IGitInformation<StaticGitInformation>, IInitializer
{
    private readonly IModuleContext<StaticGitInformation> _context;

    public StaticGitInformation(IModuleContext<StaticGitInformation> context)
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
        DefaultBranchName = (await RunCommands("rev-parse", "--abbrev-ref", "origin/HEAD")).Replace("origin/", string.Empty);
        LastCommit = await RunCommands("log", "-1");
        LastCommitSha = await RunCommands("rev-parse", "HEAD");
        LastCommitShortSha = await  RunCommands("rev-parse", "--short", "HEAD");
        Tag =  await RunCommands("describe", "--tags");
        CommitsOnBranch =  int.Parse(await RunCommands("rev-list", "HEAD", "--count"));
        LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(await RunCommands("log", "-1", "--format=%at")));
    }
    
    private async Task<string> RunCommands(params string[] commands)
    {
        var commandResult = await _context.Command.UsingCommandLineTool(
            new CommandLineToolOptions("git")
            {
                Arguments = commands
            });

        return commandResult.StandardOutput.Trim();
    }
    
    public string BranchName { get; private set; } = null!;
    public string DefaultBranchName { get; private set; } = null!;

    public string Tag { get; private set; } = null!;

    public string LastCommit { get; private set; } = null!;

    public async Task<IEnumerable<string>> LastCommits(int count)
    {
        var result = await RunCommands("log", $"-{count}");
        return result.Split(Environment.NewLine);
    }

    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
        
    public string LastCommitSha { get; private set; } = null!;

    public string LastCommitShortSha { get; private set; } = null!;
}