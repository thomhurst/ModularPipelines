using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public class GitInformation<T> : IGitInformation<T>
{
    private readonly IModuleContext<T> _context;

    public GitInformation(IModuleContext<T> context)
    {
        _context = context;
    }
    
    public string BranchName => RunCommands(new []{ "rev-parse", "--abbrev-ref", "HEAD" });term
    public string Tag => GitVersionInformation.PreReleaseTag;
    public string Label => GitVersionInformation.PreReleaseLabel;
    public int CommitsOnBranch => int.Parse(GitVersionInformation.CommitsSinceVersionSource);
    public DateOnly CommitDate => DateOnly.Parse(GitVersionInformation.CommitDate);
    public string Sha => GitVersionInformation.Sha;

    private string RunCommands(IEnumerable<string> commands) => _context.Command.UsingCommandLineTool(
        new CommandLineToolOptions("git")
        {
            Arguments = commands
        }).Result.StandardOutput;
}