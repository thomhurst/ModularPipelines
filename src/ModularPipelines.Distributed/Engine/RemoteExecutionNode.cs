using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Engine;

/// <summary>
/// Executes modules remotely on a worker node.
/// </summary>
internal sealed class RemoteExecutionNode : IExecutionNode
{
    private readonly WorkerNode _worker;
    private readonly IRemoteCommunicator _communicator;
    private readonly ModuleSerializer _moduleSerializer;
    private readonly ContextSerializer _contextSerializer;
    private readonly ILogger<RemoteExecutionNode> _logger;

    public RemoteExecutionNode(
        WorkerNode worker,
        IRemoteCommunicator communicator,
        ModuleSerializer moduleSerializer,
        ContextSerializer contextSerializer,
        ILogger<RemoteExecutionNode> logger)
    {
        _worker = worker ?? throw new ArgumentNullException(nameof(worker));
        _communicator = communicator ?? throw new ArgumentNullException(nameof(communicator));
        _moduleSerializer = moduleSerializer ?? throw new ArgumentNullException(nameof(moduleSerializer));
        _contextSerializer = contextSerializer ?? throw new ArgumentNullException(nameof(contextSerializer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        NodeId = worker.Id;
    }

    /// <inheritdoc />
    public string NodeId { get; }

    /// <inheritdoc />
    public bool CanExecute(ModuleBase module)
    {
        ArgumentNullException.ThrowIfNull(module);

        // Check if worker has capacity
        if (_worker.CurrentLoad >= _worker.Capabilities.MaxParallelModules)
        {
            _logger.LogDebug(
                "Worker {WorkerId} is at capacity ({CurrentLoad}/{MaxLoad})",
                _worker.Id,
                _worker.CurrentLoad,
                _worker.Capabilities.MaxParallelModules);
            return false;
        }

        // Check if worker is available
        if (_worker.Status != WorkerStatus.Available)
        {
            _logger.LogDebug(
                "Worker {WorkerId} is not available. Status: {Status}",
                _worker.Id,
                _worker.Status);
            return false;
        }

        // Check module requirements
        if (!CheckModuleRequirements(module))
        {
            return false;
        }

        return true;
    }

    private bool CheckModuleRequirements(ModuleBase module)
    {
        var moduleType = module.GetType();

        // Check OS requirements
        var osRequirements = moduleType.GetCustomAttributes(typeof(Attributes.RequiresOsAttribute), inherit: true)
            .Cast<Attributes.RequiresOsAttribute>()
            .ToList();

        if (osRequirements.Count > 0)
        {
            // If any RequiresOs attribute is present, check if worker OS matches any of them (OR logic)
            var matchesAnyOs = osRequirements.Any(attr => attr.OperatingSystem == _worker.Capabilities.Os);

            if (!matchesAnyOs)
            {
                _logger.LogDebug(
                    "Worker {WorkerId} OS {WorkerOs} does not match module {ModuleType} required OS(es): {RequiredOsValues}",
                    _worker.Id,
                    _worker.Capabilities.Os,
                    moduleType.Name,
                    string.Join(", ", osRequirements.Select(r => r.OperatingSystem)));
                return false;
            }
        }

        // Check tool requirements
        var toolRequirements = moduleType.GetCustomAttributes(typeof(Attributes.RequiresToolAttribute), inherit: true)
            .Cast<Attributes.RequiresToolAttribute>()
            .ToList();

        foreach (var toolReq in toolRequirements)
        {
            if (!_worker.Capabilities.InstalledTools.Contains(toolReq.ToolName, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogDebug(
                    "Worker {WorkerId} does not have required tool '{ToolName}' for module {ModuleType}",
                    _worker.Id,
                    toolReq.ToolName,
                    moduleType.Name);
                return false;
            }
        }

        // Check tag requirements
        var tagRequirements = moduleType.GetCustomAttributes(typeof(Attributes.RequiresTagAttribute), inherit: true)
            .Cast<Attributes.RequiresTagAttribute>()
            .ToList();

        foreach (var tagReq in tagRequirements)
        {
            if (!_worker.Capabilities.Tags.Contains(tagReq.Tag, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogDebug(
                    "Worker {WorkerId} does not have required tag '{Tag}' for module {ModuleType}",
                    _worker.Id,
                    tagReq.Tag,
                    moduleType.Name);
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<IModuleResult> ExecuteAsync(
        ModuleBase module,
        IReadOnlyDictionary<Type, IModuleResult> dependencyResults,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(dependencyResults);

        var executionId = Guid.NewGuid().ToString();

        try
        {
            _worker.CurrentLoad++;

            _logger.LogInformation(
                "Executing module {ModuleType} remotely on worker {WorkerId} (execution {ExecutionId})",
                module.GetType().Name,
                _worker.Id,
                executionId);

            // Serialize the module
            var serializedModule = _moduleSerializer.SerializeModule(module);

            // Serialize dependency results
            var serializedDependencies = _moduleSerializer.SerializeDependencyResults(dependencyResults);

            // Extract environment variables
            var environmentVariables = _contextSerializer.ExtractEnvironmentVariables();

            // Create execution request
            var request = new ModuleExecutionRequest
            {
                ExecutionId = executionId,
                SerializedModule = serializedModule,
                ModuleTypeName = module.GetType().AssemblyQualifiedName ?? module.GetType().FullName!,
                DependencyResults = serializedDependencies,
                EnvironmentVariables = environmentVariables,
                WorkingDirectory = _contextSerializer.GetWorkingDirectory(),
                Timeout = module.Timeout != TimeSpan.Zero ? module.Timeout : null,
            };

            // Send execution request to worker
            var response = await _communicator.ExecuteModuleAsync(_worker, request, cancellationToken);

            if (!response.Success)
            {
                var exception = new Exception(
                    $"Remote execution failed: {response.ErrorMessage}");

                _logger.LogError(
                    "Module {ModuleType} failed on worker {WorkerId}. Error: {Error}",
                    module.GetType().Name,
                    _worker.Id,
                    response.ErrorMessage);

                throw exception;
            }

            // Deserialize the result
            var result = _moduleSerializer.DeserializeResult(response.SerializedResult!);

            _logger.LogInformation(
                "Module {ModuleType} completed on worker {WorkerId}. Duration: {Duration}",
                module.GetType().Name,
                _worker.Id,
                response.Duration);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to execute module {ModuleType} on worker {WorkerId}",
                module.GetType().Name,
                _worker.Id);
            throw;
        }
        finally
        {
            _worker.CurrentLoad--;
        }
    }

    /// <inheritdoc />
    public int GetCurrentLoad()
    {
        return _worker.CurrentLoad;
    }
}
