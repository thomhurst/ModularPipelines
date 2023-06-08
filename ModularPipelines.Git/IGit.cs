namespace ModularPipelines.Git;

public interface IGit
{
    IGitOperations Operations { get; }
    IGitInformation Information { get; }
}

public interface IGit<T> : IGit
{
}