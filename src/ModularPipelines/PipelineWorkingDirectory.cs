namespace ModularPipelines;

internal sealed record PipelineWorkingDirectory(string Path)
{
    public string ResolvePath(string path) =>
        System.IO.Path.IsPathFullyQualified(path)
            ? path
            : System.IO.Path.GetFullPath(path, Path);
}
