$ErrorActionPreference = "Stop"
$srcPath = "C:\git\ModularPipelines\.worktrees\fix-1811\src"

# Find all service directories in tool packages (excluding Docker which is already done)
$servicesDirs = @(
    "$srcPath\ModularPipelines.AmazonWebServices\Services",
    "$srcPath\ModularPipelines.Azure\Generated\Services",
    "$srcPath\ModularPipelines.DotNet\Generated\Services",
    "$srcPath\ModularPipelines.Git\Services",
    "$srcPath\ModularPipelines.Google\Services",
    "$srcPath\ModularPipelines.Helm\Services",
    "$srcPath\ModularPipelines.Kubernetes\Generated\Services",
    "$srcPath\ModularPipelines.WinGet\Services",
    "$srcPath\ModularPipelines.Terraform\Services",
    "$srcPath\ModularPipelines.Yarn\Services",
    "$srcPath\ModularPipelines.Chocolatey\Services"
)

$updatedCount = 0
$skippedCount = 0

foreach ($dir in $servicesDirs) {
    if (-not (Test-Path $dir)) {
        Write-Host "Directory not found: $dir" -ForegroundColor Yellow
        continue
    }

    $files = Get-ChildItem -Path $dir -Filter '*.cs' -Recurse -ErrorAction SilentlyContinue

    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw

        # Skip if already updated
        if ($content -match 'CommandExecutionOptions') {
            $skippedCount++
            continue
        }

        # Skip if no ExecuteCommandLineTool calls (not a service file)
        if ($content -notmatch 'ExecuteCommandLineTool') {
            continue
        }

        $originalContent = $content

        # Add using statement if needed
        if ($content -match 'using ModularPipelines\.Models;' -and $content -notmatch 'using ModularPipelines\.Options;') {
            $content = $content -replace '(using ModularPipelines\.Models;)', "`$1`nusing ModularPipelines.Options;"
        }

        # Update interface method signatures
        # Pattern: Task<CommandResult> MethodName(OptionsType options, CancellationToken cancellationToken = default);
        $content = $content -replace '(Task<CommandResult>\s+\w+)\((\w+Options\??\s+options(?:(?:\s*=\s*default)?)),\s*CancellationToken\s+cancellationToken\s*=\s*default\);', '$1($2, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default);'

        # Update implementation method signatures - multi-line format
        $content = $content -replace '(public virtual async Task<CommandResult>\s+\w+\(\s*\r?\n\s+)(\w+Options\??\s+options(?:\s*=\s*default)?),(\s*\r?\n\s+CancellationToken\s+cancellationToken\s*=\s*default\))', "`$1`$2,`n        CommandExecutionOptions? executionOptions = null,`$3"

        # Update implementation method signatures - single-line format
        $content = $content -replace '(public virtual async Task<CommandResult>\s+\w+)\((\w+Options\??\s+options(?:\s*=\s*default)?),\s*CancellationToken\s+cancellationToken\s*=\s*default\)', '$1($2, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)'

        # Update method body - with default options pattern
        $content = $content -replace 'return await _command\.ExecuteCommandLineTool\(options \?\? new (\w+Options)\(\), cancellationToken\);', 'return await _command.ExecuteCommandLineTool(options ?? new $1(), executionOptions, cancellationToken);'

        # Update method body - non-nullable options pattern
        $content = $content -replace 'return await _command\.ExecuteCommandLineTool\(options, cancellationToken\);', 'return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);'

        if ($content -ne $originalContent) {
            Set-Content -Path $file.FullName -Value $content -NoNewline
            Write-Host "Updated $($file.FullName)"
            $updatedCount++
        }
    }
}

Write-Host ""
Write-Host "Done! Updated $updatedCount files, skipped $skippedCount already-updated files"
