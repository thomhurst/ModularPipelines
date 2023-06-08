namespace ModularPipelines.Git;

public class Git<T> : IGit<T>
{
    public Git(IGitOperations<T> operations, IGitInformation<T> information)
    {
        Operations = operations;
        Information = information;
    }

    public IGitOperations Operations { get; }
    public IGitInformation Information { get; }
}