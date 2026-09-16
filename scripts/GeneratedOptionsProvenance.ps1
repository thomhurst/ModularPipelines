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

function Get-GeneratedOptionsSourceFingerprint {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [string]$Revision = 'HEAD'
    )

    $objectIds = @(
        foreach ($sourcePath in Get-GeneratedOptionsSourcePath) {
            if ($sourcePath -eq 'Directory.Packages.props') {
                Get-GeneratedOptionsPackageFingerprint -RepositoryRoot $RepositoryRoot -Revision $Revision
                continue
            }

            $objectId = git -C $RepositoryRoot rev-parse "${Revision}:$sourcePath"
            if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($objectId)) {
                throw "Could not resolve generated-options source '$sourcePath' at '$Revision'."
            }

            $objectId.Trim()
        }
    )

    $bytes = [Text.Encoding]::UTF8.GetBytes($objectIds -join "`n")
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
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
