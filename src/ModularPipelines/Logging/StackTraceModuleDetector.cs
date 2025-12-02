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
/// It uses several fallback strategies:
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

    public Type? DetectModuleType()
    {
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

        // Strategy 3: Check if called from Logger property getter
        var getLoggerFrame = stackFrames.FirstOrDefault(sf => sf.GetMethod()?.Name == "get_Logger");

        if (getLoggerFrame != null)
        {
            var getLoggerFrameIndex = stackFrames.IndexOf(getLoggerFrame);
            var nextFrame = stackFrames[getLoggerFrameIndex + 1];
            var type = nextFrame.GetMethod()?.ReflectedType;

            if (type != null)
            {
                if (cacheKey != null)
                {
                    _typeCache.TryAdd(cacheKey, type);
                }
                return type;
            }
        }

        // Strategy 4: Find calling class
        var callingType = GetCallingClassType(stackFrames);
        if (cacheKey != null)
        {
            _typeCache.TryAdd(cacheKey, callingType);
        }
        return callingType;
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
        return stackFrames
            .Select(x => x.GetMethod())
            .FirstOrDefault(x => x?.GetCustomAttribute<ModuleMethodMarkerAttribute>() != null)
            ?.DeclaringType;
    }

    private static bool IsModule(Type? type)
    {
        if (type is null)
        {
            return false;
        }

        return !type.IsAbstract && type.IsAssignableTo(typeof(IModule));
    }

    private static Type GetCallingClassType(List<StackFrame> stackFrames)
    {
        // Try to find a calling class from the entry assembly first
        var entryAssemblyFirstCallingClass = stackFrames
            .Select(GetNonCompilerGeneratedType)
            .OfType<Type>()
            .Where(t => t != typeof(ModuleLoggerProvider))
            .Where(t => t != typeof(StackTraceModuleDetector))
            .Where(x => x.Assembly == Assembly.GetEntryAssembly())
            .FirstOrDefault(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });

        if (entryAssemblyFirstCallingClass != null)
        {
            return entryAssemblyFirstCallingClass;
        }

        // Fallback: any non-abstract, non-generic calling class
        return stackFrames
            .Select(GetNonCompilerGeneratedType)
            .OfType<Type>()
            .Where(t => t != typeof(ModuleLoggerProvider))
            .Where(t => t != typeof(StackTraceModuleDetector))
            .First(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });
    }

    private static Type? GetNonCompilerGeneratedType(StackFrame stackFrame)
    {
        var type = stackFrame.GetMethod()?.ReflectedType;

        // Unwrap compiler-generated types (e.g., async state machines)
        while (type?.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
        {
            type = type.DeclaringType;
        }

        return type;
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
