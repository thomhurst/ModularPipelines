using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Helpers;
using ModularPipelines.Distributed.Models;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Distributed.Engine;

/// <summary>
/// Handles module execution requests on a worker node.
/// </summary>
internal sealed class WorkerModuleExecutionHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModuleSerializer _moduleSerializer;
    private readonly ContextSerializer _contextSerializer;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly IPipelineContextProvider _contextProvider;
    private readonly ILogger<WorkerModuleExecutionHandler> _logger;
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _executionCancellations = new();

    public WorkerModuleExecutionHandler(
        IServiceProvider serviceProvider,
        ModuleSerializer moduleSerializer,
        ContextSerializer contextSerializer,
        IModuleExecutor moduleExecutor,
        IPipelineContextProvider contextProvider,
        ILogger<WorkerModuleExecutionHandler> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _moduleSerializer = moduleSerializer ?? throw new ArgumentNullException(nameof(moduleSerializer));
        _contextSerializer = contextSerializer ?? throw new ArgumentNullException(nameof(contextSerializer));
        _moduleExecutor = moduleExecutor ?? throw new ArgumentNullException(nameof(moduleExecutor));
        _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executes a module based on the provided request.
    /// </summary>
    /// <param name="request">The execution request.</param>
    /// <param name="workerId">The ID of this worker.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The execution response.</returns>
    public async Task<ModuleResultResponse> ExecuteModuleAsync(
        ModuleExecutionRequest request,
        string workerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(workerId);

        var startTime = DateTimeOffset.UtcNow;
        var executionCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        // Register cancellation token for this execution
        _executionCancellations[request.ExecutionId] = executionCts;

        _logger.LogInformation(
            "Worker {WorkerId} executing module {ModuleType} (execution {ExecutionId})",
            workerId,
            request.ModuleTypeName,
            request.ExecutionId);

        try
        {
            // Apply environment variables
            if (request.EnvironmentVariables.Count > 0)
            {
                _contextSerializer.ApplyEnvironmentVariables(request.EnvironmentVariables);
            }

            // Change working directory if specified
            if (!string.IsNullOrWhiteSpace(request.WorkingDirectory))
            {
                Directory.SetCurrentDirectory(request.WorkingDirectory);
            }

            // Deserialize the module
            var moduleType = Type.GetType(request.ModuleTypeName);
            if (moduleType == null)
            {
                throw new InvalidOperationException(
                    $"Could not resolve module type: {request.ModuleTypeName}");
            }

            var module = _moduleSerializer.DeserializeModule(request.SerializedModule, moduleType);

            // Initialize module with context
            var context = _contextProvider.GetModuleContext();
            module.Initialize(context);

            // Deserialize dependency results if provided
            if (request.DependencyResults.Count > 0)
            {
                var dependencyResults = _moduleSerializer.DeserializeDependencyResults(request.DependencyResults);

                _logger.LogDebug(
                    "Module has {DependencyCount} dependencies available",
                    dependencyResults.Count);

                // TODO: Make dependency results available to the module
                // This might require extending the module initialization
            }

            // Apply timeout if specified
            if (request.Timeout.HasValue)
            {
                executionCts.CancelAfter(request.Timeout.Value);
            }

            // Execute the module
            await _moduleExecutor.ExecuteAsync(new[] { module });

            // Get the result
            var result = await module.GetModuleResult();

            var endTime = DateTimeOffset.UtcNow;
            var duration = endTime - startTime;

            // Serialize the result
            var serializedResult = _moduleSerializer.SerializeResult(result);

            // Check if module result contains files that need to be transferred
            var transferredFiles = await DetectAndPrepareFilesForTransferAsync(result, context, cancellationToken);

            _logger.LogInformation(
                "Worker {WorkerId} completed module {ModuleType} successfully. Duration: {Duration}. Transferred files: {FileCount}",
                workerId,
                moduleType.Name,
                duration,
                transferredFiles?.Count ?? 0);

            return new ModuleResultResponse
            {
                ExecutionId = request.ExecutionId,
                Success = true,
                SerializedResult = serializedResult,
                Duration = duration,
                StartTime = startTime,
                EndTime = endTime,
                WorkerId = workerId,
                TransferredFiles = transferredFiles,
            };
        }
        catch (OperationCanceledException) when (executionCts.Token.IsCancellationRequested)
        {
            var endTime = DateTimeOffset.UtcNow;
            var duration = endTime - startTime;

            _logger.LogWarning(
                "Module execution {ExecutionId} was cancelled",
                request.ExecutionId);

            return new ModuleResultResponse
            {
                ExecutionId = request.ExecutionId,
                Success = false,
                ErrorMessage = "Execution was cancelled",
                ExceptionType = nameof(OperationCanceledException),
                Duration = duration,
                StartTime = startTime,
                EndTime = endTime,
                WorkerId = workerId,
            };
        }
        catch (Exception ex)
        {
            var endTime = DateTimeOffset.UtcNow;
            var duration = endTime - startTime;

            _logger.LogError(
                ex,
                "Worker {WorkerId} failed to execute module {ModuleType} (execution {ExecutionId})",
                workerId,
                request.ModuleTypeName,
                request.ExecutionId);

            return new ModuleResultResponse
            {
                ExecutionId = request.ExecutionId,
                Success = false,
                ErrorMessage = ex.Message,
                ExceptionType = ex.GetType().FullName ?? ex.GetType().Name,
                StackTrace = ex.StackTrace,
                Duration = duration,
                StartTime = startTime,
                EndTime = endTime,
                WorkerId = workerId,
            };
        }
        finally
        {
            // Remove cancellation token
            _executionCancellations.TryRemove(request.ExecutionId, out _);
            executionCts.Dispose();
        }
    }

    /// <summary>
    /// Cancels a module execution.
    /// </summary>
    /// <param name="executionId">The execution ID to cancel.</param>
    /// <returns>True if the execution was cancelled; false if it was not found.</returns>
    public bool CancelExecution(string executionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executionId);

        if (_executionCancellations.TryGetValue(executionId, out var cts))
        {
            _logger.LogInformation(
                "Cancelling execution {ExecutionId}",
                executionId);

            cts.Cancel();
            return true;
        }

        _logger.LogWarning(
            "Execution {ExecutionId} not found for cancellation",
            executionId);

        return false;
    }

    /// <summary>
    /// Gets the number of currently executing modules.
    /// </summary>
    /// <returns>The current execution count.</returns>
    public int GetCurrentExecutionCount()
    {
        return _executionCancellations.Count;
    }

    /// <summary>
    /// Detects files in module results and prepares them for transfer to the orchestrator.
    /// </summary>
    /// <param name="result">The module result to inspect.</param>
    /// <param name="context">The pipeline context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Dictionary of files prepared for transfer, or null if no files found.</returns>
    private async Task<Dictionary<string, FileTransferInfo>?> DetectAndPrepareFilesForTransferAsync(
        ModularPipelines.Models.IModuleResult result,
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Use reflection to get the Value property from ModuleResult<T>
        var resultType = result.GetType();
        var valueProperty = resultType.GetProperty("Value");

        if (valueProperty == null)
        {
            return null;
        }

        var value = valueProperty.GetValue(result);
        if (value == null)
        {
            return null;
        }

        var filesToTransfer = new List<File>();

        // Check if value is a File
        if (value is File singleFile)
        {
            filesToTransfer.Add(singleFile);
        }
        // Check if value is IEnumerable<File> or File[]
        else if (value is IEnumerable<File> fileCollection)
        {
            filesToTransfer.AddRange(fileCollection);
        }
        // Check if value has a property that contains files (e.g., TestExecutionResult.CoverageFiles)
        else
        {
            var properties = value.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(File))
                {
                    if (prop.GetValue(value) is File file)
                    {
                        filesToTransfer.Add(file);
                    }
                }
                else if (typeof(IEnumerable<File>).IsAssignableFrom(prop.PropertyType))
                {
                    if (prop.GetValue(value) is IEnumerable<File> files)
                    {
                        filesToTransfer.AddRange(files);
                    }
                }
            }
        }

        if (filesToTransfer.Count == 0)
        {
            return null;
        }

        _logger.LogInformation(
            "Detected {FileCount} files to transfer from module result",
            filesToTransfer.Count);

        // Get git root directory as base directory, or use current directory
        var baseDirectory = context.Git().RootDirectory?.Path ?? Directory.GetCurrentDirectory();

        return await FileTransferHelper.PrepareFilesForTransferAsync(
            filesToTransfer,
            baseDirectory,
            cancellationToken);
    }
}
