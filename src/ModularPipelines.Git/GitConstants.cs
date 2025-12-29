namespace ModularPipelines.Git;

internal static class GitConstants
{
    public const string GitEscapedLineSeparator = "%%%n%%";
    public const string DotNetLineSeparator = "%\n%";

    /// <summary>
    /// Git log format for parsing commits. Fields separated by GitEscapedLineSeparator:
    /// AuthorName, AuthorEmail, AuthorDate, CommitterName, CommitterEmail, CommitterDate,
    /// FullSha, ShortSha, Subject, Body
    /// </summary>
    /// <remarks>
    /// Uses %b (body only) instead of %B (raw body) because %s already captures the subject.
    /// Using %B would duplicate the subject in the body field.
    /// </remarks>
    public const string CommitLogFormat =
        $"'%aN {GitEscapedLineSeparator} %aE {GitEscapedLineSeparator} %aI {GitEscapedLineSeparator} " +
        $"%cN {GitEscapedLineSeparator} %cE {GitEscapedLineSeparator} %cI {GitEscapedLineSeparator} " +
        $"%H {GitEscapedLineSeparator} %h {GitEscapedLineSeparator} %s {GitEscapedLineSeparator} %b'";
}
