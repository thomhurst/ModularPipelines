namespace ModularPipelines;

internal interface ISmartCollapsableLoggingStringBlockProvider
{
    string? GetStartConsoleLogGroup(string name);

    string? GetEndConsoleLogGroup(string name);
}