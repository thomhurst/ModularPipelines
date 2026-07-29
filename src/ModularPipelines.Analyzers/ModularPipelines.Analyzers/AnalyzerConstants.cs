using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Constants for type and member names used in analyzers.
/// Using constants avoids magic strings that could break if types are renamed.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class AnalyzerConstants
{
    /// <summary>
    /// Type name constants for ModularPipelines types.
    /// </summary>
    internal static class TypeNames
    {
        /// <summary>
        /// The Module base type name (generic Module&lt;T&gt;).
        /// </summary>
        internal const string Module = "Module";

        /// <summary>
        /// The DependsOn attribute type name.
        /// </summary>
        internal const string DependsOn = "DependsOn";

        /// <summary>
        /// The Task type name from System.Threading.Tasks.
        /// </summary>
        internal const string Task = "Task";

        /// <summary>
        /// The IEnumerable interface name.
        /// </summary>
        internal const string IEnumerable = "IEnumerable";
    }

    /// <summary>
    /// Method name constants used in analyzers.
    /// </summary>
    internal static class MethodNames
    {
        /// <summary>
        /// The ExecuteAsync method name from Module.
        /// </summary>
        internal const string ExecuteAsync = "ExecuteAsync";

        /// <summary>
        /// The GetModule method name for retrieving module dependencies.
        /// </summary>
        internal const string GetModule = "GetModule";

        /// <summary>
        /// The GetModuleIfRegistered method name for retrieving optional module dependencies.
        /// </summary>
        internal const string GetModuleIfRegistered = "GetModuleIfRegistered";

        /// <summary>
        /// The FromResult method name from Task.
        /// </summary>
        internal const string FromResult = "FromResult";

        /// <summary>
        /// The OnAfterExecuteAsync method name from Module.
        /// </summary>
        internal const string OnAfterExecuteAsync = "OnAfterExecuteAsync";
    }

    /// <summary>
    /// Modifier keyword constants.
    /// </summary>
    internal static class Modifiers
    {
        /// <summary>
        /// The async modifier keyword.
        /// </summary>
        internal const string Async = "async";

        /// <summary>
        /// The override modifier keyword.
        /// </summary>
        internal const string Override = "override";
    }

    /// <summary>
    /// Namespace constants used in analyzers.
    /// </summary>
    internal static class Namespaces
    {
        /// <summary>
        /// The System.Threading.Tasks namespace.
        /// </summary>
        internal const string SystemThreadingTasks = "System.Threading.Tasks";
    }

    /// <summary>
    /// Fully qualified type name constants used in analyzers.
    /// </summary>
    internal static class FullyQualifiedTypeNames
    {
        /// <summary>
        /// The metadata name for the generic Module&lt;T&gt; base type.
        /// </summary>
        internal const string Module = "ModularPipelines.Modules.Module`1";

        /// <summary>
        /// The fully qualified System.Console type in global:: format.
        /// </summary>
        internal const string SystemConsole = "global::System.Console";

        /// <summary>
        /// The prefix for IEnumerable&lt;T&gt; in global:: fully qualified format.
        /// </summary>
        internal const string IEnumerablePrefix = "global::System.Collections.Generic.IEnumerable<";
    }
}
