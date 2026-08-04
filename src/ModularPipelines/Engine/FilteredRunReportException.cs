namespace ModularPipelines.Engine;

internal interface IFilteredRunReportException
{
    string TypeName { get; }

    string? OriginalStackTrace { get; }
}

internal sealed class FilteredRunReportException : Exception, IFilteredRunReportException
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

internal sealed class FilteredRunReportAggregateException
    : AggregateException, IFilteredRunReportException
{
    public FilteredRunReportAggregateException(
        AggregateException source,
        IEnumerable<Exception> innerExceptions)
        : base(source.Message, innerExceptions)
    {
        TypeName = source is IFilteredRunReportException filtered
            ? filtered.TypeName
            : source.GetType().FullName ?? source.GetType().Name;
        OriginalStackTrace = source is IFilteredRunReportException existing
            ? existing.OriginalStackTrace
            : source.StackTrace;
    }

    public string TypeName { get; }

    public string? OriginalStackTrace { get; }
}
