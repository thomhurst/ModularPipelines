using System.Globalization;
using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

public class GitCommitMapper : IGitCommitMapper
{
    private const int ExpectedFieldCount = 10;

    public GitCommit Map(string commandLineOutput)
    {
        var lines = commandLineOutput
            .Split(GitConstants.DotNetLineSeparator, StringSplitOptions.None)
            .Select(x => x.Trim())
            .ToArray();

        if (lines.Length != ExpectedFieldCount)
        {
            throw new ArgumentException(
                $"Git commit output is malformed. Expected exactly {ExpectedFieldCount} fields but received {lines.Length}. " +
                $"Output: {commandLineOutput}",
                nameof(commandLineOutput));
        }

        return new GitCommit
        {
            Author = new GitAuthor
            {
                Name = lines[0],
                Email = lines[1],
                Date = ParseDate(lines[2]),
            },
            Committer = new GitAuthor
            {
                Name = lines[3],
                Email = lines[4],
                Date = ParseDate(lines[5]),
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

    private static DateTime ParseDate(string value)
    {
        return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture).UtcDateTime;
    }
}
