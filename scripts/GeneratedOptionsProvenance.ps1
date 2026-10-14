function Assert-GeneratedOptionsToken {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][string]$Value
    )

    if ($Value -notmatch '^[A-Za-z0-9][A-Za-z0-9._-]*$') {
        throw "$Name contains unsupported characters: '$Value'."
    }
}

function Assert-GeneratedOptionsCommandMetadata {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][AllowEmptyString()][string]$ToolVersion,
        [Parameter(Mandatory)][AllowEmptyString()][string]$CommandTreeSha256
    )

    if ([string]::IsNullOrWhiteSpace($ToolVersion) -or
        [string]::IsNullOrWhiteSpace($CommandTreeSha256)) {
        throw "$Name contains incomplete command metadata."
    }

    if ($CommandTreeSha256 -notmatch '^[0-9A-Fa-f]{64}$') {
        throw "$Name contains an invalid command tree SHA-256."
    }
}

function Get-GeneratedOptionsSourcePath {
    return @(
        '.github/workflows/generate-cli-options.yml',
        'Directory.Build.props',
        'Directory.Packages.props',
        'global.json',
        'scripts/GeneratedOptionsProvenance.ps1',
        'scripts/Write-GeneratedOptionsProvenance.ps1',
        'src/ModularPipelines/Attributes/CliOptionValueArity.cs',
        'src/ModularPipelines/Attributes/CommandLinePhase.cs',
        'src/ModularPipelines/Helpers/Internal/WindowsCommandResolver.cs',
        'src/ModularPipelines/Options/AdditionalCommandLineArgument.cs',
        'src/ModularPipelines/Options/CommandLineToolOptions.cs',
        'tools/Directory.Build.props',
        'tools/ModularPipelines.OptionsGenerator'
    )
}

$GeneratedOptionsGeneratorRoot = 'tools/ModularPipelines.OptionsGenerator'
$GeneratedOptionsGeneratorProject = "$GeneratedOptionsGeneratorRoot/src/ModularPipelines.OptionsGenerator"

# Generator-tree files that cannot change generated output. The workflow builds and runs
# only the generator executable project and the non-test scripts beside it.
function Test-GeneratedOptionsIgnoredSource {
    param([Parameter(Mandatory)][string]$RelativePath)

    # The test project is never built or run by generation.
    if ($RelativePath.StartsWith('src/ModularPipelines.OptionsGenerator.Tests/', [StringComparison]::Ordinal)) { return $true }
    # Examples and Markdown are documentation for people; the generator project embeds no Markdown.
    if ($RelativePath.StartsWith('examples/', [StringComparison]::Ordinal)) { return $true }
    if ($RelativePath.EndsWith('.md', [StringComparison]::OrdinalIgnoreCase)) { return $true }
    # Public API baselines feed an analyzer, not the emitted code.
    if ($RelativePath -match '(^|/)PublicAPI\.(Shipped|Unshipped)\.txt$') { return $true }
    # Script self-tests run in CI, not during generation.
    if ($RelativePath -match '^scripts/Test-[^/]+\.ps1$') { return $true }
    return $false
}

# Returns relative path -> tool name for files that only one tool's generation reads:
# a `<Name>CliScraper[.<Part>].cs` group whose class declares a literal ToolName, with
# the `Scrapers/Cli/<Name><Helper>.cs` files beside it, and `TypeOverrides/<tool>.json`,
# which ManualOverrideDetector loads by tool name. A scraper group stays shared when any
# other generator file names one of its top-level types. The one exception is a scraper
# class named in OptionsGeneratorCommand.cs, which registers every scraper; that file is
# itself a shared input.
function Get-GeneratedOptionsToolOwnedSource {
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Revision,
        [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$RelativePaths
    )

    $owners = @{}
    foreach ($path in $RelativePaths) {
        if ($path -match '^src/ModularPipelines\.OptionsGenerator/TypeOverrides/([^/]+)\.json$') {
            $owners[$path] = $Matches[1].ToLowerInvariant()
        }
    }

    $groups = @{}
    foreach ($path in $RelativePaths) {
        if ($path -match '^src/ModularPipelines\.OptionsGenerator/Scrapers/Cli/([A-Z][A-Za-z0-9]*CliScraper)(\.[A-Za-z0-9]+)*\.cs$') {
            if (-not $groups.ContainsKey($Matches[1])) { $groups[$Matches[1]] = [Collections.Generic.List[string]]::new() }
            $groups[$Matches[1]].Add($path)
        }
    }
    if ($groups.Count -eq 0) { return $owners }

    # A helper such as GcloudSynopsisGroupReconciler.cs joins the group with the longest
    # matching name prefix. The next character must start a new word, so `Go` does not
    # claim `GoogleX.cs`.
    $groupNames = @($groups.Keys | Sort-Object { $_.Length } -Descending)
    foreach ($path in $RelativePaths) {
        if ($path -notmatch '^src/ModularPipelines\.OptionsGenerator/Scrapers/Cli/([A-Z][A-Za-z0-9]*)(\.[A-Za-z0-9]+)*\.cs$') { continue }
        $name = $Matches[1]
        if ($name.EndsWith('CliScraper', [StringComparison]::Ordinal)) { continue }
        foreach ($group in $groupNames) {
            $stem = $group.Substring(0, $group.Length - 'CliScraper'.Length)
            if ($name.Length -gt $stem.Length -and $name.StartsWith($stem, [StringComparison]::Ordinal) -and [char]::IsUpper($name[$stem.Length])) {
                $groups[$group].Add($path)
                break
            }
        }
    }

    # One search reads every scraper's tool name, namespace style, and type declarations.
    $prefix = "${Revision}:$GeneratedOptionsGeneratorRoot/"
    $lines = @(git -C $RepositoryRoot grep -n -E `
            'override[[:space:]]+string[[:space:]]+ToolName|^namespace[[:space:]]|^ *([a-z]+ +)*(class|record|struct|interface|enum) +[A-Z]' `
            $Revision -- "$GeneratedOptionsGeneratorProject/Scrapers/Cli/*.cs")
    if ($LASTEXITCODE -gt 1) { throw "Could not search generated-options scrapers at '$Revision'." }
    $fileScoped = @{}
    $declarations = [Collections.Generic.List[object]]::new()
    $groupTool = @{}
    $pathGroup = @{}
    foreach ($group in $groups.Keys) { foreach ($path in $groups[$group]) { $pathGroup[$path] = $group } }
    foreach ($hit in $lines) {
        if (-not $hit.StartsWith($prefix, [StringComparison]::Ordinal)) { continue }
        $path, $null, $text = $hit.Substring($prefix.Length) -split ':', 3
        $group = $pathGroup[$path]
        if (-not $group) { continue }
        if ($text -match 'override\s+string\s+ToolName\s*=>\s*"([^"]+)"') { $groupTool[$group] = $Matches[1].ToLowerInvariant() }
        elseif ($text -match '^namespace\s+[\w.]+\s*;') { $fileScoped[$path] = $true }
        elseif ($text -match '^( *)(?:[a-z]+\s+)*(?:class|record|struct|interface|enum)\s+([A-Z]\w*)') {
            $declarations.Add([pscustomobject]@{ Path = $path; Indent = $Matches[1].Length; Name = $Matches[2] })
        }
    }

    # File-scoped namespaces put top-level types at column 0; block namespaces indent them once.
    $typeOwner = @{}
    foreach ($d in $declarations) {
        $topLevelIndent = if ($fileScoped[$d.Path]) { 0 } else { 4 }
        if ($d.Indent -eq $topLevelIndent) { $typeOwner[$d.Name] = $pathGroup[$d.Path] }
    }

    # A group without a literal tool name (a shared base such as CobraCliScraper) is shared.
    $shared = [Collections.Generic.HashSet[string]]::new()
    foreach ($group in $groups.Keys) {
        if (-not $groupTool.ContainsKey($group)) { [void]$shared.Add($group) }
    }

    if ($typeOwner.Count -gt 0) {
        $names = ($typeOwner.Keys | Sort-Object) -join '|'
        $pattern = "\b($names)\b"
        $hits = @(git -C $RepositoryRoot grep -n -w -E "($names)" $Revision -- "$GeneratedOptionsGeneratorProject/*.cs")
        if ($LASTEXITCODE -gt 1) { throw "Could not search generated-options sources at '$Revision'." }
        foreach ($hit in $hits) {
            if (-not $hit.StartsWith($prefix, [StringComparison]::Ordinal)) { continue }
            $path, $null, $line = $hit.Substring($prefix.Length) -split ':', 3
            if (Test-GeneratedOptionsIgnoredSource -RelativePath $path) { continue }
            foreach ($m in [regex]::Matches($line, $pattern)) {
                $group = $typeOwner[$m.Value]
                if ($pathGroup[$path] -eq $group) { continue }
                if ($path -eq 'src/ModularPipelines.OptionsGenerator/OptionsGeneratorCommand.cs' -and
                    $m.Value.EndsWith('CliScraper', [StringComparison]::Ordinal)) { continue }
                [void]$shared.Add($group)
            }
        }
    }

    foreach ($group in $groups.Keys) {
        if ($shared.Contains($group)) { continue }
        foreach ($path in $groups[$group]) { $owners[$path] = $groupTool[$group] }
    }
    return $owners
}

# Returns sorted `path<TAB>object` entries for every source that can affect one tool's
# generated output. Other tools' scrapers and overrides are left out, so their changes do
# not make this tool's snapshots stale.
function Get-GeneratedOptionsSourceEntry {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Tool,
        [string]$Revision = 'HEAD'
    )

    $tool = $Tool.ToLowerInvariant()
    $entries = [Collections.Generic.List[string]]::new()
    foreach ($sourcePath in Get-GeneratedOptionsSourcePath) {
        if ($sourcePath -eq $GeneratedOptionsGeneratorRoot) { continue }
        if ($sourcePath -eq 'Directory.Packages.props') {
            $entries.Add("$sourcePath`t" + (Get-GeneratedOptionsPackageFingerprint -RepositoryRoot $RepositoryRoot -Revision $Revision))
            continue
        }

        $objectId = git -C $RepositoryRoot rev-parse "${Revision}:$sourcePath"
        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($objectId)) {
            throw "Could not resolve generated-options source '$sourcePath' at '$Revision'."
        }
        $entries.Add("$sourcePath`t$($objectId.Trim())")
    }

    $tree = @(git -C $RepositoryRoot ls-tree -r --full-tree $Revision -- $GeneratedOptionsGeneratorRoot)
    if ($LASTEXITCODE -ne 0 -or $tree.Count -eq 0) {
        throw "Could not resolve generated-options source '$GeneratedOptionsGeneratorRoot' at '$Revision'."
    }
    $files = [ordered]@{}
    foreach ($line in $tree) {
        $meta, $path = $line -split "`t", 2
        $relative = $path.Substring($GeneratedOptionsGeneratorRoot.Length + 1)
        if (Test-GeneratedOptionsIgnoredSource -RelativePath $relative) { continue }
        $files[$relative] = ($meta -split ' ')[2]
    }

    $owners = Get-GeneratedOptionsToolOwnedSource -RepositoryRoot $RepositoryRoot -Revision $Revision -RelativePaths @($files.Keys)
    foreach ($relative in $files.Keys) {
        $owner = $owners[$relative]
        if ($owner -and $owner -ne $tool) { continue }
        $entries.Add("$GeneratedOptionsGeneratorRoot/$relative`t$($files[$relative])")
    }

    # The hash must not depend on the culture of the machine that computes it.
    $sorted = $entries.ToArray()
    [Array]::Sort($sorted, [StringComparer]::Ordinal)
    return $sorted
}

function Get-GeneratedOptionsSourceFingerprint {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Tool,
        [string]$Revision = 'HEAD'
    )

    $entries = Get-GeneratedOptionsSourceEntry -RepositoryRoot $RepositoryRoot -Tool $Tool -Revision $Revision
    $bytes = [Text.Encoding]::UTF8.GetBytes($entries -join "`n")
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
}

# Names the sources that differ between two revisions for one tool, for a stale diagnostic.
function Get-GeneratedOptionsChangedSource {
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Tool,
        [Parameter(Mandatory)][string]$FromRevision,
        [Parameter(Mandatory)][string]$ToRevision
    )

    $from = @{}
    foreach ($e in Get-GeneratedOptionsSourceEntry -RepositoryRoot $RepositoryRoot -Tool $Tool -Revision $FromRevision) {
        $path, $id = $e -split "`t", 2
        $from[$path] = $id
    }
    $changed = [Collections.Generic.List[string]]::new()
    foreach ($e in Get-GeneratedOptionsSourceEntry -RepositoryRoot $RepositoryRoot -Tool $Tool -Revision $ToRevision) {
        $path, $id = $e -split "`t", 2
        if ($from[$path] -ne $id) { $changed.Add($path) }
        $from.Remove($path)
    }
    $changed.AddRange([string[]]@($from.Keys))
    $sorted = $changed.ToArray()
    [Array]::Sort($sorted, [StringComparer]::Ordinal)
    return $sorted
}

function Get-GeneratedOptionsXmlAtRevision {
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Revision,
        [Parameter(Mandatory)][string]$Path
    )

    $contents = git -C $RepositoryRoot show "${Revision}:$Path"
    if ($LASTEXITCODE -ne 0) {
        throw "Could not read generated-options build input '$Path' at '$Revision'."
    }

    return [xml]($contents -join "`n")
}

function Get-GeneratedOptionsPackageFingerprint {
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(Mandatory)][string]$Revision
    )

    $centralPackages = Get-GeneratedOptionsXmlAtRevision -RepositoryRoot $RepositoryRoot `
        -Revision $Revision -Path 'Directory.Packages.props'
    $referenceNames = @{
        PackageReference = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        GlobalPackageReference = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    }
    $referencePaths = @(
        'Directory.Build.props',
        'tools/Directory.Build.props',
        'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj'
    )
    $buildDocuments = @(
        $centralPackages
        foreach ($referencePath in $referencePaths) {
            Get-GeneratedOptionsXmlAtRevision -RepositoryRoot $RepositoryRoot `
                -Revision $Revision -Path $referencePath
        }
    )

    # Visit shared imports before the executable project, matching their build order.
    # Conditional includes are conservative; only unconditional removes discard them.
    # Test-project references cannot affect the generator executable's restore.
    $filterVersions = $true
    foreach ($document in $buildDocuments) {
        foreach ($reference in $document.SelectNodes(
            '//*[local-name()="PackageReference" or local-name()="GlobalPackageReference"][@Include or @Remove]')) {
            $isInclude = $reference.HasAttribute('Include')
            $name = if ($isInclude) { $reference.GetAttribute('Include') }
                else { $reference.GetAttribute('Remove') }
            if ($name -notmatch '^[A-Za-z0-9_.-]+$') { $filterVersions = $false }
            $names = $referenceNames[$reference.LocalName]
            if ($isInclude) {
                [void]$names.Add($name)
            } elseif (-not $reference.SelectSingleNode(
                'ancestor-or-self::*[@Condition or local-name()="When" or local-name()="Otherwise"]')) {
                [void]$names.Remove($name)
            }
        }

        # Transitive pinning can make an otherwise unreferenced central version relevant.
        foreach ($pinning in $document.SelectNodes('//*[local-name()="CentralPackageTransitivePinningEnabled"]')) {
            if ($pinning.InnerText.Trim() -ne 'false') { $filterVersions = $false }
        }
    }

    # Retain all versions when computed names make static filtering unsafe.
    $versions = @($centralPackages.SelectNodes('//*[local-name()="PackageVersion"][@Include or @Update]'))
    foreach ($version in $versions) {
        $name = if ($version.HasAttribute('Include')) { $version.GetAttribute('Include') }
            else { $version.GetAttribute('Update') }
        if ($name -notmatch '^[A-Za-z0-9_.-]+$') { $filterVersions = $false }
    }
    if ($filterVersions) {
        $packageNames = $referenceNames.PackageReference
        $packageNames.UnionWith($referenceNames.GlobalPackageReference)
        foreach ($version in $versions) {
            $name = if ($version.HasAttribute('Include')) { $version.GetAttribute('Include') }
                else { $version.GetAttribute('Update') }
            if (-not $packageNames.Contains($name)) {
                [void]$version.ParentNode.RemoveChild($version)
            }
        }
    }

    # Preserve central settings and conditions along with the relevant version nodes.
    $bytes = [Text.Encoding]::UTF8.GetBytes($centralPackages.OuterXml)
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
}
