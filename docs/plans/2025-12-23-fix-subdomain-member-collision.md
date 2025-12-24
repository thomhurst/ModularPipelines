# Fix SubDomain Member Collision Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Fix CS0102 duplicate member errors in options generator by moving colliding commands to child class `Execute()` methods instead of generating both a property and method with the same name.

**Architecture:** When a CLI command exists as both a sub-command group (property) AND a standalone command (method), detect the collision and pass the command to the child class as its `parentCommand`, which generates an `Execute()` method. This leverages the existing `GenerateExecuteMethod` pattern.

**Tech Stack:** C#, .NET, ModularPipelines.OptionsGenerator

---

### Task 1: Add Static Method Name Helper

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs:258-271`

**Step 1: Extract GetMethodName to static helper**

The existing `GetMethodName` method is already static but takes a `CommandTreeNode` parameter. We need it accessible before we have the node context. Add this helper method after the existing `GetMethodName` method:

```csharp
private static string GetMethodNameFromCommand(CliCommandDefinition command)
{
    if (command.CommandParts.Length == 0)
    {
        return "Execute";
    }

    var lastPart = command.CommandParts[^1];
    var parts = lastPart.Split('-', StringSplitOptions.RemoveEmptyEntries);
    return string.Join("", parts.Select(p =>
        char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p[1..].ToLowerInvariant() : "")));
}
```

**Step 2: Verify the file compiles**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs
git commit -m "refactor: extract GetMethodNameFromCommand helper in SubDomainClassGenerator"
```

---

### Task 2: Detect Colliding Commands in GenerateFilesFromTree

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs:44-67`

**Step 1: Add collision detection logic**

Replace the `GenerateFilesFromTree` method with this updated version that detects and handles collisions:

```csharp
private static void GenerateFilesFromTree(
    CommandTreeNode node,
    CliToolDefinition tool,
    List<GeneratedFile> files,
    CliCommandDefinition? parentCommand = null)
{
    // Build map of commands that collide with child property names
    // These will become Execute() methods on the child classes instead
    var collidingCommands = new Dictionary<string, CliCommandDefinition>(StringComparer.OrdinalIgnoreCase);
    foreach (var command in node.Commands)
    {
        var methodName = GetMethodNameFromCommand(command);
        var matchingChild = node.Children.Values
            .FirstOrDefault(c => c.PascalSegment.Equals(methodName, StringComparison.OrdinalIgnoreCase));

        if (matchingChild != null)
        {
            collidingCommands[matchingChild.Segment] = command;
        }
    }

    // Generate file for this node
    // Pass parentCommand only for root nodes (depth 0), and pass excluded commands
    var content = GenerateNodeClass(
        node,
        tool,
        node.Depth == 0 ? parentCommand : null,
        collidingCommands.Values.ToHashSet());

    var fileName = $"{node.ClassName}.cs";
    var relativePath = Path.Combine(tool.OutputDirectory, "Services", fileName);

    files.Add(new GeneratedFile
    {
        RelativePath = relativePath,
        Content = content
    });

    // Recursively generate files for children
    // Pass colliding command as parentCommand so it becomes Execute() on the child
    foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
    {
        collidingCommands.TryGetValue(child.Segment, out var childParentCommand);
        GenerateFilesFromTree(child, tool, files, childParentCommand);
    }
}
```

**Step 2: Verify the file compiles (will fail - GenerateNodeClass signature mismatch)**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj`
Expected: Build error (GenerateNodeClass needs updated signature)

---

### Task 3: Update GenerateNodeClass to Accept Excluded Commands

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs:69-164`

**Step 1: Update GenerateNodeClass signature and filter commands**

Update the method signature to accept excluded commands and filter them out:

```csharp
private static string GenerateNodeClass(
    CommandTreeNode node,
    CliToolDefinition tool,
    CliCommandDefinition? parentCommand = null,
    HashSet<CliCommandDefinition>? excludedCommands = null)
{
    var sb = new StringBuilder();

    // File header
    GeneratorUtils.GenerateFileHeader(sb);

    sb.AppendLine("using ModularPipelines.Context;");
    sb.AppendLine("using ModularPipelines.Models;");
    sb.AppendLine("using ModularPipelines.Options;");
    sb.AppendLine($"using {tool.TargetNamespace}.Options;");
    sb.AppendLine();

    // Namespace
    sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
    sb.AppendLine();

    // Class documentation
    sb.AppendLine($"/// <summary>");
    sb.AppendLine($"/// {tool.ToolName} {node.Segment.ToLowerInvariant()} commands.");
    sb.AppendLine($"/// </summary>");
    sb.AppendLine($"public class {node.ClassName}");
    sb.AppendLine("{");

    // Private field for ICommand
    sb.AppendLine("    private readonly ICommand _command;");

    // Private fields for child instances (lazy)
    foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
    {
        sb.AppendLine($"    private {child.ClassName}? _{char.ToLowerInvariant(child.PascalSegment[0])}{child.PascalSegment[1..]};");
    }

    sb.AppendLine();

    // Constructor
    sb.AppendLine($"    /// <summary>");
    sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{node.ClassName}\"/> class.");
    sb.AppendLine($"    /// </summary>");
    sb.AppendLine($"    public {node.ClassName}(ICommand command)");
    sb.AppendLine("    {");
    sb.AppendLine("        _command = command;");
    sb.AppendLine("    }");

    // Properties for child sub-command groups
    if (node.Children.Count > 0)
    {
        sb.AppendLine();
        sb.AppendLine("    #region Sub-command Groups");
        sb.AppendLine();

        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            var fieldName = $"_{char.ToLowerInvariant(child.PascalSegment[0])}{child.PascalSegment[1..]}";

            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// {tool.ToolName} {child.Segment.ToLowerInvariant()} sub-commands.");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    public {child.ClassName} {child.PascalSegment} => {fieldName} ??= new {child.ClassName}(_command);");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
    }

    // Filter out excluded commands (they become Execute() on child classes)
    var commandsToGenerate = excludedCommands != null
        ? node.Commands.Where(c => !excludedCommands.Contains(c)).ToList()
        : node.Commands;

    // Methods for direct commands at this level
    var hasCommands = commandsToGenerate.Count > 0 || parentCommand is not null;
    if (hasCommands)
    {
        sb.AppendLine();
        sb.AppendLine("    #region Commands");
        sb.AppendLine();

        // Add Execute method for the parent command if it exists
        if (parentCommand is not null)
        {
            GenerateExecuteMethod(sb, parentCommand);
            sb.AppendLine();
        }

        foreach (var command in commandsToGenerate.OrderBy(c => c.ClassName))
        {
            GenerateMethod(sb, command, node);
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
    }

    sb.AppendLine("}");

    return sb.ToString();
}
```

**Step 2: Verify the project compiles**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs
git commit -m "fix: handle colliding commands by moving to child Execute() methods

When a CLI command name collides with a sub-command group property name,
the command is now passed to the child class as its parentCommand, which
generates an Execute() method. This avoids CS0102 duplicate member errors.

Example: gcloud.AccessApproval.Requests property + Requests() method
becomes: gcloud.AccessApproval.Requests property + Requests.Execute() method"
```

---

### Task 4: Regenerate gcloud Options and Verify Build

**Files:**
- Regenerate: `src/ModularPipelines.Google/Services/*.cs`
- Regenerate: `src/ModularPipelines.Google/Options/*.cs`

**Step 1: Run the options generator for gcloud**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools gcloud --output-dir src/ModularPipelines.Google`
Expected: Generation completes without errors

**Step 2: Build ModularPipelines.Google to verify no CS0102 errors**

Run: `dotnet build src/ModularPipelines.Google/ModularPipelines.Google.csproj`
Expected: Build succeeded (no CS0102 duplicate member errors)

**Step 3: Spot-check generated file for correct pattern**

Run: `findstr /C:"Execute(" src\ModularPipelines.Google\Services\GcloudAccessApprovalRequests.cs`
Expected: Should find Execute() method in the child class

Run: `findstr /C:"Requests(" src\ModularPipelines.Google\Services\GcloudAccessApproval.cs`
Expected: Should NOT find Requests() method (it moved to child class)

**Step 4: Commit generated files**

```bash
git add src/ModularPipelines.Google/
git commit -m "chore: regenerate gcloud options with collision fix"
```

---

### Task 5: Build Full Solution and Run Tests

**Step 1: Build the full solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeded

**Step 2: Run unit tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --no-build -c Release`
Expected: All tests pass

**Step 3: Final commit if any additional changes needed**

If tests reveal issues, fix and commit. Otherwise, the fix is complete.

---

## Verification Checklist

- [ ] `SubDomainClassGenerator.cs` has `GetMethodNameFromCommand` helper
- [ ] `GenerateFilesFromTree` detects colliding commands and passes to children
- [ ] `GenerateNodeClass` filters out excluded commands
- [ ] `GcloudAccessApproval.cs` has `Requests` property but NO `Requests()` method
- [ ] `GcloudAccessApprovalRequests.cs` has `Execute()` method for the requests command
- [ ] `ModularPipelines.Google` builds without CS0102 errors
- [ ] Full solution builds successfully
- [ ] Unit tests pass
