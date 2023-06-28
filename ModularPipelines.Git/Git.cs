namespace ModularPipelines.Git;

public class Git : IGit
{
    public Git(IGitOperations operations, IGitInformation information)
    {
        Operations = operations;
        Information = information;
    }

    public IGitOperations Operations { get; }
    public IGitInformation Information { get; }
}