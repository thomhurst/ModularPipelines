using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

internal class GitCommitMapper : IGitCommitMapper
{
    public GitCommit Map(string commandLineOutput)
    {
        var lines = commandLineOutput.Split(GitConstants.DotNetLineSeparator, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

        return new GitCommit
        {
            Author = new GitAuthor
            {
                Name = lines[0],
                Email = lines[1],
                Date = DateTime.Parse(lines[2]),
            },
            Committer = new GitAuthor
            {
                Name = lines[3],
                Email = lines[4],
                Date = DateTime.Parse(lines[5]),
            },
            Hash = new GitHash
            {
                Long = lines[6],
                Short = lines[7],
            },
            Message = new GitMessage
            {
                Subject = lines[8],
                Body = lines[9],
            },
        };
    }
}
