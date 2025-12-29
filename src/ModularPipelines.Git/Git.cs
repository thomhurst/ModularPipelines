using ModularPipelines.FileSystem;

namespace ModularPipelines.Git;

internal class Git : IGit
{
    public Git(IGitCommands commands,
        IGitInformation information,
        IGitVersioning versioning)
    {
        Commands = commands;
        Information = information;
        Versioning = versioning;
    }

    public IGitCommands Commands { get; }

    public IGitInformation Information { get; }

    public IGitVersioning Versioning { get; }

    public Folder RootDirectory => Information.Root ?? throw new InvalidOperationException(
        "Git repository root directory not detected. Ensure this code is running from within a Git repository.");
}