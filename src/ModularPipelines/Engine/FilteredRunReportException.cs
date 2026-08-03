namespace ModularPipelines.Engine;

internal sealed class FilteredRunReportException : Exception
{
    public FilteredRunReportException(Exception source, Exception? innerException)
        : base(source.Message, innerException)
    {
        TypeName = source is FilteredRunReportException filtered
            ? filtered.TypeName
            : source.GetType().FullName ?? source.GetType().Name;
        OriginalStackTrace = source is FilteredRunReportException existing
            ? existing.OriginalStackTrace
            : source.StackTrace;
    }

    public string TypeName { get; }

    public string? OriginalStackTrace { get; }
}
