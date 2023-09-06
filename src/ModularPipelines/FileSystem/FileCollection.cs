using System.Collections;
using ModularPipelines.Extensions;

namespace ModularPipelines.FileSystem;

public class FileCollection : IEnumerable<File>
{
    private readonly IEnumerable<File> _enumerableImplementation;

    internal FileCollection(IEnumerable<File> enumerableImplementation)
    {
        _enumerableImplementation = enumerableImplementation;
    }

    public IEnumerator<File> GetEnumerator()
    {
        return _enumerableImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _enumerableImplementation.GetEnumerator();
    }

    public static implicit operator List<string>(FileCollection fileCollection) => fileCollection.AsPaths().ToList();
}