# SubModule API Documentation

## Overview

The SubModule API provides a way to create named sub-tasks within your modules. This is particularly useful for:

- Processing items in loops with clear progress indication
- Parallel processing with individual task identification
- Structured logging that shows exactly what's being processed
- Performance monitoring with clear operation boundaries

## Quick Start

```csharp
public class ProcessFilesModule : Module<string[]>
{
    public override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var files = new[] { "file1.txt", "file2.txt", "file3.txt" };

        return await files.ToAsyncProcessorBuilder()
            .SelectAsync(file => SubModule(context, file, async () =>
            {
                context.Logger.LogInformation("Processing {File}", file);
                var content = await File.ReadAllTextAsync(file);
                return content.ToUpper();
            }, cancellationToken))
            .ProcessInParallel();
    }
}
```

## API Signatures

The SubModule API provides 4 overloads to cover all scenarios:

### 1. Async with Result

Execute an asynchronous operation that returns a value.

```csharp
protected Task<TResult> SubModule<TResult>(
    IPipelineContext context,
    string name,
    Func<Task<TResult>> action,
    CancellationToken cancellationToken = default)
```

**Parameters**:
- `context`: The pipeline context (provides logging and services)
- `name`: Descriptive name for the sub-task
- `action`: Async function to execute
- `cancellationToken`: Optional cancellation token

**Returns**: Task with the result of the action

**Example**:
```csharp
var result = await SubModule(context, "Download File", async () =>
{
    var content = await httpClient.GetStringAsync(url);
    return content;
}, cancellationToken);
```

### 2. Sync with Result

Execute a synchronous operation that returns a value.

```csharp
protected Task<TResult> SubModule<TResult>(
    IPipelineContext context,
    string name,
    Func<TResult> action,
    CancellationToken cancellationToken = default)
```

**Parameters**:
- `context`: The pipeline context (provides logging and services)
- `name`: Descriptive name for the sub-task
- `action`: Sync function to execute
- `cancellationToken`: Optional cancellation token

**Returns**: Task with the result of the action

**Example**:
```csharp
var result = await SubModule(context, "Calculate Hash", () =>
{
    using var sha256 = SHA256.Create();
    return Convert.ToBase64String(sha256.ComputeHash(data));
}, cancellationToken);
```

### 3. Async without Result

Execute an asynchronous operation that doesn't return a value.

```csharp
protected Task SubModule(
    IPipelineContext context,
    string name,
    Func<Task> action,
    CancellationToken cancellationToken = default)
```

**Parameters**:
- `context`: The pipeline context (provides logging and services)
- `name`: Descriptive name for the sub-task
- `action`: Async function to execute
- `cancellationToken`: Optional cancellation token

**Returns**: Task representing the async operation

**Example**:
```csharp
await SubModule(context, "Upload File", async () =>
{
    await ftpClient.UploadFileAsync(localPath, remotePath);
}, cancellationToken);
```

### 4. Sync without Result

Execute a synchronous operation that doesn't return a value.

```csharp
protected Task SubModule(
    IPipelineContext context,
    string name,
    Action action,
    CancellationToken cancellationToken = default)
```

**Parameters**:
- `context`: The pipeline context (provides logging and services)
- `name`: Descriptive name for the sub-task
- `action`: Sync action to execute
- `cancellationToken`: Optional cancellation token

**Returns**: Task representing the operation

**Example**:
```csharp
await SubModule(context, "Write Config", () =>
{
    File.WriteAllText(configPath, configContent);
}, cancellationToken);
```

## Logging

SubModule automatically provides structured logging with the format:

```
[SubModule] ModuleName.SubModuleName starting
[SubModule] ModuleName.SubModuleName completed
```

Or in case of errors:

```
[SubModule] ModuleName.SubModuleName failed
```

### Log Levels

- **Starting**: LogDebug
- **Completed**: LogDebug
- **Failed**: LogError (includes exception details)

### Example Output

```
[SubModule] ProcessFilesModule.file1.txt starting
[SubModule] ProcessFilesModule.file1.txt completed
[SubModule] ProcessFilesModule.file2.txt starting
[SubModule] ProcessFilesModule.file2.txt completed
[SubModule] ProcessFilesModule.file3.txt starting
[SubModule] ProcessFilesModule.file3.txt completed
```

## Use Cases

### 1. Parallel File Processing

Process multiple files in parallel with clear progress indication:

```csharp
public class ProcessImagesModule : Module<ImageResult[]>
{
    public override async Task<ImageResult[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var imageFiles = context.FileSystem.RootDirectory
            .GetFolder("images")
            .FindFiles(x => x.Extension is ".jpg" or ".png");

        return await imageFiles.ToAsyncProcessorBuilder()
            .SelectAsync(file => SubModule(context, file.Name, async () =>
            {
                using var image = await Image.LoadAsync(file.Path);

                // Resize
                image.Mutate(x => x.Resize(800, 600));

                // Save
                var outputPath = Path.Combine("output", file.Name);
                await image.SaveAsync(outputPath);

                return new ImageResult
                {
                    OriginalPath = file.Path,
                    ProcessedPath = outputPath
                };
            }, cancellationToken))
            .ProcessInParallel(maxDegreeOfParallelism: 4);
    }
}
```

### 2. Sequential Processing with Progress

Process items sequentially with clear progress tracking:

```csharp
public class DeployServicesModule : Module<DeploymentResult[]>
{
    public override async Task<DeploymentResult[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var services = new[] { "api", "web", "worker" };
        var results = new List<DeploymentResult>();

        foreach (var service in services)
        {
            var result = await SubModule(context, $"Deploy {service}", async () =>
            {
                context.Logger.LogInformation("Deploying {Service}...", service);

                // Build Docker image
                await context.Docker().Build(new DockerBuildOptions
                {
                    Dockerfile = $"./services/{service}/Dockerfile",
                    Tag = $"{service}:latest"
                }, cancellationToken);

                // Push to registry
                await context.Docker().Push($"{service}:latest", cancellationToken);

                // Deploy to Kubernetes
                await context.Kubectl().Apply($"./k8s/{service}.yaml", cancellationToken);

                return new DeploymentResult
                {
                    Service = service,
                    Success = true,
                    Timestamp = DateTime.UtcNow
                };
            }, cancellationToken);

            results.Add(result);
        }

        return results.ToArray();
    }
}
```

### 3. Data Transformation Pipeline

Transform data through multiple stages with clear operation boundaries:

```csharp
public class DataProcessingModule : Module<ProcessedData>
{
    public override async Task<ProcessedData?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Stage 1: Extract
        var rawData = await SubModule(context, "Extract Data", async () =>
        {
            return await context.Http().GetAsync<RawData>("https://api.example.com/data");
        }, cancellationToken);

        // Stage 2: Transform
        var transformedData = await SubModule(context, "Transform Data", () =>
        {
            return rawData.Select(item => new TransformedData
            {
                Id = item.Id,
                Value = item.RawValue.ToUpper(),
                Processed = DateTime.UtcNow
            }).ToList();
        }, cancellationToken);

        // Stage 3: Load
        await SubModule(context, "Load Data", async () =>
        {
            await database.BulkInsertAsync(transformedData);
        }, cancellationToken);

        return new ProcessedData
        {
            RecordCount = transformedData.Count,
            ProcessedAt = DateTime.UtcNow
        };
    }
}
```

### 4. API Batch Operations

Make multiple API calls with clear tracking:

```csharp
public class SyncUsersModule : Module<UserSyncResult[]>
{
    public override async Task<UserSyncResult[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var userIds = await GetUserIdsToSync();

        return await userIds.ToAsyncProcessorBuilder()
            .SelectAsync(userId => SubModule(context, $"User {userId}", async () =>
            {
                try
                {
                    var user = await apiClient.GetUserAsync(userId);
                    await database.UpsertUserAsync(user);

                    return new UserSyncResult
                    {
                        UserId = userId,
                        Success = true
                    };
                }
                catch (Exception ex)
                {
                    context.Logger.LogError(ex, "Failed to sync user {UserId}", userId);

                    return new UserSyncResult
                    {
                        UserId = userId,
                        Success = false,
                        Error = ex.Message
                    };
                }
            }, cancellationToken))
            .ProcessInParallel(maxDegreeOfParallelism: 10);
    }
}
```

## Best Practices

### 1. Use Descriptive Names

Choose names that clearly describe what the sub-task is doing:

```csharp
// Good
await SubModule(context, "Download dependencies", ...)
await SubModule(context, $"Process {file.Name}", ...)
await SubModule(context, "Validate configuration", ...)

// Bad
await SubModule(context, "Step 1", ...)
await SubModule(context, "Do stuff", ...)
await SubModule(context, "Task", ...)
```

### 2. Include Context in Names

For loop processing, include the item identifier in the name:

```csharp
// Good
await SubModule(context, $"Build {project.Name}", ...)
await SubModule(context, $"Deploy to {environment}", ...)
await SubModule(context, $"Test {testSuite.Name}", ...)

// Less Helpful
await SubModule(context, "Build project", ...)
await SubModule(context, "Deploy", ...)
await SubModule(context, "Test", ...)
```

### 3. Keep SubModules Focused

Each SubModule should represent a single logical operation:

```csharp
// Good - Separate concerns
var downloaded = await SubModule(context, "Download", async () =>
    await DownloadFile(url));

var processed = await SubModule(context, "Process", () =>
    ProcessData(downloaded));

await SubModule(context, "Upload", async () =>
    await UploadResult(processed));

// Bad - Too many responsibilities
await SubModule(context, "Do Everything", async () =>
{
    var data = await DownloadFile(url);
    var processed = ProcessData(data);
    await UploadResult(processed);
});
```

### 4. Use Appropriate Overloads

Choose the right overload for your operation:

```csharp
// Async with result - when you need async and a return value
var result = await SubModule(context, "Download", async () =>
    await DownloadAsync());

// Sync with result - when you have sync code with a return value
var hash = await SubModule(context, "Hash", () =>
    ComputeHash(data));

// Async without result - when you need async but no return value
await SubModule(context, "Upload", async () =>
    await UploadAsync(data));

// Sync without result - when you have sync code with no return value
await SubModule(context, "Log", () =>
    File.AppendAllText(logFile, message));
```

### 5. Handle Exceptions Appropriately

Let SubModule handle the logging, but catch exceptions when you need custom handling:

```csharp
// Let SubModule log exceptions
await SubModule(context, "Risky Operation", async () =>
{
    await RiskyOperationAsync(); // Exception will be logged and re-thrown
});

// Custom exception handling when needed
var result = await SubModule(context, "Tolerant Operation", async () =>
{
    try
    {
        return await OperationAsync();
    }
    catch (ExpectedException ex)
    {
        context.Logger.LogWarning("Expected failure: {Message}", ex.Message);
        return DefaultValue;
    }
});
```

## Error Handling

SubModule automatically:
1. Logs exceptions with LogError
2. Includes the module name and sub-task name
3. Re-throws the exception

**Example Error Log**:
```
[ERROR] [SubModule] ProcessFilesModule.file1.txt failed
System.InvalidOperationException: Unable to process file
   at ProcessFilesModule.<ExecuteAsync>...
```

## Performance Considerations

### Parallel Processing

SubModule works seamlessly with parallel processing libraries:

```csharp
// Process in parallel with degree of parallelism
await items.ToAsyncProcessorBuilder()
    .SelectAsync(item => SubModule(context, item.Name, () => Process(item), cancellationToken))
    .ProcessInParallel(maxDegreeOfParallelism: 4);
```

### Progress Tracking

Each SubModule logs start and completion, providing automatic progress tracking:

```
[SubModule] MyModule.Item1 starting
[SubModule] MyModule.Item2 starting
[SubModule] MyModule.Item1 completed
[SubModule] MyModule.Item3 starting
[SubModule] MyModule.Item2 completed
...
```

## Comparison with Direct Execution

### Without SubModule

```csharp
foreach (var file in files)
{
    context.Logger.LogInformation("Processing {File}", file);
    try
    {
        await ProcessFileAsync(file);
        context.Logger.LogInformation("Completed {File}", file);
    }
    catch (Exception ex)
    {
        context.Logger.LogError(ex, "Failed processing {File}", file);
        throw;
    }
}
```

### With SubModule

```csharp
foreach (var file in files)
{
    await SubModule(context, file, async () =>
    {
        await ProcessFileAsync(file);
    }, cancellationToken);
}
```

**Benefits**:
- Less boilerplate
- Consistent logging format
- Automatic exception handling
- Clear operation boundaries

## Integration with Pipeline Features

### With Retry Policies

SubModule works with module retry policies:

```csharp
public class RetryModule : Module<string>, IModuleRetryPolicy
{
    public IAsyncPolicy GetRetryPolicy()
    {
        return Policy
            .Handle<Exception>()
            .RetryAsync(3);
    }

    public override async Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // SubModule calls will be retried if the module fails
        return await SubModule(context, "Operation", async () =>
        {
            return await RiskyOperationAsync();
        }, cancellationToken);
    }
}
```

### With Progress Reporting

SubModule automatically participates in pipeline progress reporting:

```csharp
var options = new PipelineOptions
{
    ShowProgressInConsole = true
};

// SubModule operations will be visible in progress output
```

## Limitations

1. **No Nested SubModules**: SubModules cannot contain other SubModules
2. **Same Thread**: SubModules execute on the calling thread
3. **No Independent Retry**: SubModules cannot have independent retry policies (module-level only)
4. **No Independent Timeout**: SubModules share the module's timeout

## Migration from v2.x

See the [V2 to V3 Migration Guide](../migration/V2_TO_V3_MIGRATION.md#submodule-api-changes) for detailed migration instructions.

**Quick Summary**:
```csharp
// v2.x
await SubModule("name", action)

// v3.0
await SubModule(context, "name", action, cancellationToken)
```

## Related Documentation

- [V2 to V3 Migration Guide](../migration/V2_TO_V3_MIGRATION.md)
- [Module Documentation](./MODULES.md)
- [Pipeline Context Documentation](./PIPELINE_CONTEXT.md)

---

**Version**: 3.0.0
**Last Updated**: 2025-11-09
