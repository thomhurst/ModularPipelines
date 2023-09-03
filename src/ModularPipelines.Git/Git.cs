using ModularPipelines.Context;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Git;

internal class Git : IGit
{
    private readonly IEnvironmentContext _environmentContext;

    public Git(IGitCommands commands, 
        IGitInformation information, 
        IGitVersioning versioning,
        IEnvironmentContext environmentContext)
    {
        _environmentContext = environmentContext;
        Commands = commands;
        Information = information;
        Versioning = versioning;
    }

    public IGitCommands Commands { get; }
    public IGitInformation Information { get; }
    public IGitVersioning Versioning { get; }

    public Folder RootDirectory => _environmentContext.GitRootDirectory ?? throw new Exception("Git directory not detected");
}