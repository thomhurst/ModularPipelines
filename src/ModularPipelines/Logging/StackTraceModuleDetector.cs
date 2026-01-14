using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Logging;

/// <summary>
/// Detects the module type from the current call stack using stack trace inspection.
/// Uses multiple detection strategies including marker attributes, module base types, and calling class analysis.
/// Results are cached to avoid repeated reflection operations.
/// </summary>
/// <remarks>
/// This class employs reflection and stack frame inspection to determine which module is requesting a logger.
///
/// Performance optimization: The detector first checks ModuleLogger.CurrentModuleType (AsyncLocal) for a fast path.
/// When the AsyncLocal is set (during module execution), expensive stack trace inspection is completely avoided.
///
/// Fallback strategies (when AsyncLocal is not available):
/// 1. Look for methods with [ModuleMethodMarker] attribute
/// 2. Find types inheriting from ModuleBase
/// 3. Find the calling class from the entry assembly
/// 4. Find any non-abstract, non-generic calling class
///
/// Detection results are cached by calling method signature to improve performance on repeated calls.
/// </remarks>
/// <example>
/// <code>
/// var detector = new StackTraceModuleDetector();
/// var moduleType = detector.DetectModuleType();
///
/// if (moduleType != null)
/// {
///     // Create logger for this module type
/// }
/// </code>
/// </example>
internal class StackTraceModuleDetector : IStackTraceModuleDetector
{
    private readonly ConcurrentDictionary<string, Type?> _typeCache = new();

    /// <summary>
    /// Cache for HasModuleMethodMarkerAttribute lookups to avoid repeated reflection calls.
    /// </summary>
    private static readonly ConcurrentDictionary<MethodBase, bool> HasMarkerAttributeCache = new();

    /// <summary>
    /// Cache for CompilerGeneratedAttribute lookups to avoid repeated reflection calls.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, bool> IsCompilerGeneratedCache = new();

    /// <summary>
    /// Cache for IsModule checks to avoid repeated IsAssignableTo calls.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, bool> IsModuleCache = new();

    /// <summary>
    /// Cached reference to the entry assembly to avoid repeated GetEntryAssembly calls.
    /// </summary>
    private static readonly Assembly? EntryAssembly = Assembly.GetEntryAssembly();

    public Type? DetectModuleType()
    {
        // Fast path: check AsyncLocal first to avoid expensive stack trace inspection
        var asyncLocalType = ModuleLogger.CurrentModuleType.Value;
        if (asyncLocalType != null)
        {
            return asyncLocalType;
        }

        // Slow path: fall back to stack trace inspection when AsyncLocal context is not available
        var stackFrames = new StackTrace().GetFrames().ToList();

        // Create cache key from the first relevant calling frame
        var cacheKey = GetCacheKey(stackFrames);
        if (cacheKey != null && _typeCache.TryGetValue(cacheKey, out var cachedType))
        {
            return cachedType;
        }

        // Strategy 1: Look for marker attributes
        var moduleFromMarker = GetModuleFromMarkerAttributes(stackFrames);
        if (moduleFromMarker != null)
        {
            if (cacheKey != null)
            {
                _typeCache.TryAdd(cacheKey, moduleFromMarker);
            }

            return moduleFromMarker;
        }

        // Strategy 2: Find module base types
        var moduleFromBase = stackFrames
            .Select(GetNonCompilerGeneratedType)
            .FirstOrDefault(IsModule);

        if (moduleFromBase != null)
        {
            if (cacheKey != null)
            {
                _typeCache.TryAdd(cacheKey, moduleFromBase);
            }

            return moduleFromBase;
        }

        // Strategy 3: Check if called from Logger property getter on a module
        var getLoggerFrame = stackFrames.FirstOrDefault(sf => sf.GetMethod()?.Name == "get_Logger");

        if (getLoggerFrame != null)
        {
            var getLoggerFrameIndex = stackFrames.IndexOf(getLoggerFrame);
            var nextFrameIndex = getLoggerFrameIndex + 1;

            if (nextFrameIndex < stackFrames.Count)
            {
                var nextFrame = stackFrames[nextFrameIndex];
                var type = nextFrame.GetMethod()?.ReflectedType;

                // Only return if this is actually a module type
                if (type != null && IsModule(type))
                {
                    if (cacheKey != null)
                    {
                        _typeCache.TryAdd(cacheKey, type);
                    }

                    return type;
                }
            }
        }

        // No module found in call stack - return null
        // Callers should handle this gracefully (e.g., use unattributed buffer)
        if (cacheKey != null)
        {
            _typeCache.TryAdd(cacheKey, null);
        }

        return null;
    }

    private static string? GetCacheKey(List<StackFrame> stackFrames)
    {
        // Find the first frame that's not from logging infrastructure
        var relevantFrame = stackFrames
            .Select(sf => sf.GetMethod())
            .FirstOrDefault(m => m?.DeclaringType != null
                && m.DeclaringType != typeof(StackTraceModuleDetector)
                && m.DeclaringType != typeof(ModuleLoggerProvider)
                && !m.DeclaringType.FullName?.StartsWith("ModularPipelines.Logging") == true);

        if (relevantFrame == null)
        {
            return null;
        }

        // Create a unique key from the method signature
        return $"{relevantFrame.DeclaringType?.FullName}.{relevantFrame.Name}";
    }

    private static Type? GetModuleFromMarkerAttributes(List<StackFrame> stackFrames)
    {
        foreach (var frame in stackFrames)
        {
            var method = frame.GetMethod();
            if (method == null)
            {
                continue;
            }

            var hasMarker = HasMarkerAttributeCache.GetOrAdd(method, static m =>
                m.GetCustomAttribute<ModuleMethodMarkerAttribute>() != null);

            if (hasMarker)
            {
                return method.DeclaringType;
            }
        }

        return null;
    }

    private static bool IsModule(Type? type)
    {
        if (type is null)
        {
            return false;
        }

        return IsModuleCache.GetOrAdd(type, static t =>
            !t.IsAbstract && t.IsAssignableTo(typeof(IModule)));
    }

    private static Type? GetNonCompilerGeneratedType(StackFrame stackFrame)
    {
        var type = stackFrame.GetMethod()?.ReflectedType;

        // Unwrap compiler-generated types (e.g., async state machines)
        while (type != null && IsCompilerGeneratedType(type))
        {
            type = type.DeclaringType;
        }

        return type;
    }

    private static bool IsCompilerGeneratedType(Type type)
    {
        return IsCompilerGeneratedCache.GetOrAdd(type, static t =>
            t.GetCustomAttribute<CompilerGeneratedAttribute>() != null);
    }
}

/// <summary>
/// Interface for detecting module types from the call stack.
/// </summary>
internal interface IStackTraceModuleDetector
{
    /// <summary>
    /// Detects the module type from the current call stack.
    /// </summary>
    /// <returns>The detected module type, or null if no module could be detected.</returns>
    Type? DetectModuleType();
}
