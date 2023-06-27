namespace ModularPipelines.Git.Models;

public record GitCommit
{
    public GitHash Hash { get; set; }
    public GitAuthor Author { get; set; }
    public GitAuthor Committer { get; set; }
    public GitMessage Message { get; set; }
}

public record GitFileStatus
{
    public string Status { get; set; }
    public string File { get; set; }
}

public record GitAuthor
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}

public record GitMessage
{
    public string Subject { get; set; }
    public string Body { get; set; }
}

public record GitHash
{
    public string Long { get; set; }
    public string Short { get; set; }
}