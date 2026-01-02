using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

public sealed class Command : ICommand
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PlaceholderPropertyInfo>> PlaceholderPropertyCache = new();

    private readonly ICommandLogger _commandLogger;
    private readonly ICommandModelProvider _commandModelProvider;
    private readonly ICommandArgumentBuilder _commandArgumentBuilder;

    public Command(
        ICommandLogger commandLogger,
        ICommandModelProvider commandModelProvider,
        ICommandArgumentBuilder commandArgumentBuilder)
    {
        _commandLogger = commandLogger;
        _commandModelProvider = commandModelProvider;
        _commandArgumentBuilder = commandArgumentBuilder;
    }

    public async Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var optionsObject = GetOptionsObject(options);

        // Get subcommand parts and handle placeholder replacement
        var precedingArgs = GetPrecedingArguments(optionsObject);

        // Build arguments from the command model using the new services
        var commandModel = _commandModelProvider.GetCommandModel(optionsObject.GetType());
        var propertyArgs = _commandArgumentBuilder.BuildArguments(commandModel, optionsObject);

        // Combine: preceding args (subcommands) + property args
        var parsedArgs = precedingArgs.Concat(propertyArgs).ToList();

        // Add any manual arguments passed via options.Arguments
        var manualArgs = (string.Equals(options.Arguments?.ElementAtOrDefault(0), options.Tool)
            ? options.Arguments?.Skip(1).ToList() : options.Arguments?.ToList()) ?? new List<string>();
        parsedArgs.AddRange(manualArgs);

        if (options.RunSettings != null)
        {
            parsedArgs.Add("--");
            parsedArgs.AddRange(options.RunSettings);
        }

        string tool;
        if (options.Sudo)
        {
            tool = "sudo";
            parsedArgs.Insert(0, options.Tool);
        }
        else
        {
            tool = options.Tool;
        }

        var command = Cli.Wrap(tool).WithArguments(parsedArgs);

        if (options.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
        }

        if (options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables));
        }

        if (options.InternalDryRun)
        {
            _commandLogger.Log(
                options: options,
                inputToLog: options.InputLoggingManipulator == null ? command.ToString() : options.InputLoggingManipulator(command.ToString()),
                exitCode: 0,
                runTime: TimeSpan.Zero,
                standardOutput: "Dummy Output Response",
                standardError: "Dummy Error Response",
                commandWorkingDirPath: command.WorkingDirPath
            );

            return new CommandResult(command);
        }

        return await Of(command, options, cancellationToken).ConfigureAwait(false);
    }

    // Note: Placeholder sanitization is no longer needed since ReplacePlaceholders
    // now handles placeholder replacement inline using CliArgumentAttribute.Name matching.
    private static List<string> GetPrecedingArguments(object optionsObject)
    {
        var rawCommandParts = GetRawCommandParts(optionsObject);
        return ReplacePlaceholders(rawCommandParts, optionsObject);
    }

    private static List<string> GetRawCommandParts(object optionsObject)
    {
        if (optionsObject is CommandLineToolOptions { CommandParts: not null } commandLineToolOptions)
        {
            return commandLineToolOptions.CommandParts.ToList();
        }

        var type = optionsObject.GetType();

        // Try new CliCommand attribute first
        // Check for preferred alias first
        var preferredAlias = type.GetCustomAttributes<CliCommandAliasAttribute>()
            .FirstOrDefault(a => a.IsPreferred);

        if (preferredAlias is not null)
        {
            return preferredAlias.CommandParts.ToList();
        }

        // Prefer the explicit CliSubCommand attribute if it exists (for classes that inherit tool from base)
        var cliSubCommandAttribute = type.GetCustomAttribute<CliSubCommandAttribute>();
        if (cliSubCommandAttribute is not null)
        {
            return cliSubCommandAttribute.SubCommands.ToList();
        }

        // Fall back to full CliCommand attribute (defines both tool and subcommands)
        var cliCommandAttribute = type.GetCustomAttribute<CliCommandAttribute>();
        if (cliCommandAttribute is not null)
        {
            // Only return SubCommands, not the tool name (which is already used by Cli.Wrap)
            return cliCommandAttribute.SubCommands.ToList();
        }

        return new List<string>();
    }

    /// <summary>
    /// Replaces placeholder strings in command parts with actual argument values.
    /// Placeholders are matched to properties via CliArgumentAttribute.Name.
    /// </summary>
    private static List<string> ReplacePlaceholders(List<string> commandParts, object optionsObject)
    {
        if (commandParts.Count == 0)
        {
            return commandParts;
        }

        // Build a lookup of placeholder name -> property value
        var placeholderValues = BuildPlaceholderValueLookup(optionsObject);

        var result = new List<string>();
        foreach (var part in commandParts)
        {
            // Check if this is a placeholder (starts with < or [<)
            if (IsPlaceholder(part))
            {
                // Try to find a matching argument value
                if (placeholderValues.TryGetValue(part, out var values) && values.Count > 0)
                {
                    result.AddRange(values);
                }

                // If no value found, skip the placeholder (it's optional)
            }
            else
            {
                // Literal command part, add as-is
                result.Add(part);
            }
        }

        return result;
    }

    private static bool IsPlaceholder(string part)
    {
        return part.StartsWith('<') || part.StartsWith("[<");
    }

    private static Dictionary<string, List<string>> BuildPlaceholderValueLookup(object optionsObject)
    {
        var lookup = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var type = optionsObject.GetType();

        // Get cached placeholder properties for this type
        var placeholderProperties = PlaceholderPropertyCache.GetOrAdd(type, BuildPlaceholderPropertyCache);

        foreach (var placeholderProperty in placeholderProperties)
        {
            var rawValue = placeholderProperty.Getter(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            var values = GetArgumentValues(rawValue);
            if (values.Count > 0)
            {
                lookup[placeholderProperty.Name] = values;
            }
        }

        return lookup;
    }

    private static IReadOnlyList<PlaceholderPropertyInfo> BuildPlaceholderPropertyCache(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var placeholderProperties = new List<PlaceholderPropertyInfo>();

        foreach (var property in properties)
        {
            var cliArgument = property.GetCustomAttribute<CliArgumentAttribute>();
            if (cliArgument?.Name is null)
            {
                continue;
            }

            var getter = CreatePropertyGetter(type, property);
            placeholderProperties.Add(new PlaceholderPropertyInfo(cliArgument.Name, getter));
        }

        return placeholderProperties;
    }

    private static Func<object, object?> CreatePropertyGetter(Type type, PropertyInfo property)
    {
        // Create a compiled expression tree for fast property access
        // Expression: (object obj) => (object?)((TType)obj).Property
        var parameter = Expression.Parameter(typeof(object), "obj");
        var castToType = Expression.Convert(parameter, type);
        var propertyAccess = Expression.Property(castToType, property);
        var castToObject = Expression.Convert(propertyAccess, typeof(object));

        var lambda = Expression.Lambda<Func<object, object?>>(castToObject, parameter);
        return lambda.Compile();
    }

    private static List<string> GetArgumentValues(object rawValue)
    {
        var result = new List<string>();

        if (rawValue is string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                result.Add(stringValue);
            }
        }
        else if (rawValue is IEnumerable enumerable and not IEnumerable<char>)
        {
            foreach (var item in enumerable)
            {
                if (item is not null)
                {
                    var itemStr = item.ToString();
                    if (!string.IsNullOrEmpty(itemStr))
                    {
                        result.Add(itemStr);
                    }
                }
            }
        }
        else
        {
            var str = rawValue.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                result.Add(str);
            }
        }

        return result;
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options.OptionsObject ?? options;
    }

    private async Task<CommandResult> Of(CliWrap.Command command,
        CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        StringBuilder standardOutputStringBuilder = new();
        StringBuilder standardErrorStringBuilder = new();
        var stopwatch = Stopwatch.StartNew();

        var standardOutput = string.Empty;
        var standardError = string.Empty;

        var inputToLog = options.InputLoggingManipulator == null ? command.ToString() : options.InputLoggingManipulator(command.ToString());

        // Only create timeout token if ExecutionTimeout is specified to avoid unnecessary allocations
        using var timeoutCancellationToken = options.ExecutionTimeout.HasValue
            ? new CancellationTokenSource(options.ExecutionTimeout.Value)
            : null;

        // Link the timeout token with the passed cancellation token, or just wrap the original if no timeout
        using var linkedCancellationToken = timeoutCancellationToken != null
            ? CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationToken.Token, cancellationToken)
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        using var forcefulCancellationToken = new CancellationTokenSource();

        await using var registration = linkedCancellationToken.Token.Register(() =>
        {
            try
            {
                if (forcefulCancellationToken.Token.CanBeCanceled)
                {
                    forcefulCancellationToken.CancelAfter(options.GracefulShutdownTimeout);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignored
            }
        });

        try
        {
            var result = await command
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(standardOutputStringBuilder))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(standardErrorStringBuilder))
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync(forcefulCancellationToken.Token, linkedCancellationToken.Token).ConfigureAwait(false);

            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: result.ExitCode,
                runTime: result.RunTime,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );

            if (result.ExitCode != 0 && options.ThrowOnNonZeroExitCode)
            {
                var input = inputToLog;
                throw new CommandException(input, result.ExitCode, result.RunTime, standardOutput, standardError);
            }

            return new CommandResult(command, result, standardOutput, standardError);
        }
        catch (CommandExecutionException e)
        {
            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: e.ExitCode,
                runTime: stopwatch.Elapsed,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );

            throw new CommandException(inputToLog, e.ExitCode, stopwatch.Elapsed, standardOutput, standardError, e);
        }
        catch (Exception e) when (e is not CommandExecutionException and not CommandException)
        {
            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: -1,
                runTime: stopwatch.Elapsed,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );

            throw new CommandException(inputToLog, -1, stopwatch.Elapsed, standardOutput, standardError, e);
        }
    }

    /// <summary>
    /// Cached information about a property that provides placeholder values.
    /// </summary>
    /// <param name="Name">The placeholder name (from CliArgumentAttribute.Name).</param>
    /// <param name="Getter">A compiled delegate to get the property value.</param>
    private sealed record PlaceholderPropertyInfo(string Name, Func<object, object?> Getter);
}
