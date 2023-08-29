using System.Collections;
using ModularPipelines.Extensions;

namespace ModularPipelines.FileSystem;

public class FolderCollection : IEnumerable<Folder>
{
    private readonly IEnumerable<Folder> _enumerableImplementation;

    internal FolderCollection(IEnumerable<Folder> enumerableImplementation)
    {
        _enumerableImplementation = enumerableImplementation;
    }

    public IEnumerator<Folder> GetEnumerator()
    {
        return _enumerableImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _enumerableImplementation.GetEnumerator();
    }

    public static implicit operator List<string>(FolderCollection folderCollection) => folderCollection.AsPaths().ToList();
}