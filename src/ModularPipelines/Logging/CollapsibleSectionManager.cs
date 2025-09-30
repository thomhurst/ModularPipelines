namespace ModularPipelines.Logging;

/// <summary>
/// Manages collapsible log sections for module logging.
/// Handles the creation of build system-specific collapsible blocks around module output.
/// </summary>
/// <remarks>
/// This class coordinates with the build system (GitHub Actions, Azure Pipelines, etc.)
/// to create collapsible sections in CI/CD logs, improving readability by grouping
/// related log messages together.
/// </remarks>
/// <example>
/// <code>
/// var manager = new CollapsibleSectionManager(blockProvider, consoleWriter);
///
/// // Start a collapsible section
/// manager.StartSection("MyModule");
///
/// // ... log content ...
///
/// // End the section
/// manager.EndSection("MyModule");
/// </code>
/// </example>
internal class CollapsibleSectionManager : ICollapsibleSectionManager
{
    private readonly ISmartCollapsableLoggingStringBlockProvider _blockProvider;
    private readonly IConsoleWriter _consoleWriter;

    public CollapsibleSectionManager(
        ISmartCollapsableLoggingStringBlockProvider blockProvider,
        IConsoleWriter consoleWriter)
    {
        _blockProvider = blockProvider;
        _consoleWriter = consoleWriter;
    }

    public void StartSection(string sectionName)
    {
        var startCommand = _blockProvider.GetStartConsoleLogGroup(sectionName);

        if (startCommand != null)
        {
            _consoleWriter.LogToConsole(startCommand);
        }
    }

    public void EndSection(string sectionName)
    {
        var endCommand = _blockProvider.GetEndConsoleLogGroup(sectionName);

        if (endCommand != null)
        {
            _consoleWriter.LogToConsole(endCommand);
        }
    }

    public string FormatSectionName(string moduleName, Exception? exception)
    {
        if (exception != null)
        {
            return $"{moduleName} - Error! {exception.GetType().Name}";
        }

        return moduleName;
    }
}

/// <summary>
/// Interface for managing collapsible log sections.
/// </summary>
internal interface ICollapsibleSectionManager
{
    /// <summary>
    /// Starts a collapsible section with the specified name.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    void StartSection(string sectionName);

    /// <summary>
    /// Ends a collapsible section with the specified name.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    void EndSection(string sectionName);

    /// <summary>
    /// Formats a section name based on the module name and optional exception.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <param name="exception">The exception, if any.</param>
    /// <returns>The formatted section name.</returns>
    string FormatSectionName(string moduleName, Exception? exception);
}
