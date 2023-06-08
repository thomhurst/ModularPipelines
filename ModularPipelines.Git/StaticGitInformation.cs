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
        
        BranchName = await RunCommandsOrNull("rev-parse", "--abbrev-ref", "HEAD");
        DefaultBranchName = (await RunCommandsOrNull("rev-parse", "--abbrev-ref", "origin/HEAD")).Replace("origin/", string.Empty);
        LastCommit = await RunCommandsOrNull("log", "-1");
        LastCommitSha = await RunCommandsOrNull("rev-parse", "HEAD");
        LastCommitShortSha = await  RunCommandsOrNull("rev-parse", "--short", "HEAD");
        Tag =  await RunCommandsOrNull("describe", "--tags");
        CommitsOnBranch =  int.Parse(await RunCommandsOrNull("rev-list", "HEAD", "--count") ?? "0");
        LastCommitDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(await RunCommandsOrNull("log", "-1", "--format=%at") ?? "0"));
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

    private async Task<string?> RunCommandsOrNull(params string[] commands)
    {
        try
        {
            return await RunCommands(commands);
        }
        catch
        {
            return null;
        }
    }
    
    public string? BranchName { get; private set; } = null!;
    public string? DefaultBranchName { get; private set; } = null!;

    public string? Tag { get; private set; } = null!;

    public string? LastCommit { get; private set; } = null!;

    public async Task<IEnumerable<string>> LastCommits(int count)
    {
        var result = await RunCommands("log", $"-{count}");
        return result.Split(Environment.NewLine);
    }

    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
        
    public string? LastCommitSha { get; private set; } = null!;

    public string? LastCommitShortSha { get; private set; } = null!;
}