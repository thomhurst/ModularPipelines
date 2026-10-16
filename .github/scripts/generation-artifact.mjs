import { randomUUID } from 'node:crypto';
import { spawnSync } from 'node:child_process';
import { appendFileSync, lstatSync, mkdirSync, readFileSync, readdirSync, rmSync, writeFileSync } from 'node:fs';
import path from 'node:path';
import { pathToFileURL } from 'node:url';

const artifactName = 'generated-options.json';
const maximumArtifactBytes = 512 * 1024 * 1024;
const maximumContentBytes = 256 * 1024 * 1024;
const maximumFiles = 50000;
const maximumReportBytes = 65536;

export function selectPublicationArtifact(jobs, artifacts, tool) {
  // A publisher-only rerun can reuse a successful generation attempt. A newer
  // failed generation attempt must never fall back to an older successful one.
  const latest = jobs.filter(job => job.name === `Generate ${tool}`)
    .sort((left, right) => right.run_attempt - left.run_attempt || right.id - left.id)[0];
  if (!latest || latest.conclusion !== 'success') return null;
  const matches = artifacts.filter(artifact =>
    artifact.name === `generated-${tool}-publication-${latest.run_attempt}` && !artifact.expired);
  if (matches.length !== 1) throw new Error('Missing or ambiguous publication artifact');
  return { id: matches[0].id, runAttempt: String(latest.run_attempt) };
}

function git(root, args, input) {
  const result = spawnSync('git', ['-C', root, ...args], { input, maxBuffer: maximumArtifactBytes });
  if (result.status !== 0) throw new Error(`git ${args[0]} failed: ${result.stderr}`);
  return result.stdout;
}

function readBlobs(root, revisions) {
  if (revisions.length === 0) return [];
  const output = git(root, ['cat-file', '--batch'], `${revisions.join('\n')}\n`);
  let offset = 0;
  return revisions.map(revision => {
    const end = output.indexOf(10, offset);
    const header = output.subarray(offset, end).toString('utf8');
    const match = /^[a-f0-9]{40} blob ([0-9]+)$/.exec(header);
    if (!match) throw new Error(`Expected tracked file: ${revision}`);
    const length = Number(match[1]);
    offset = end + 1;
    const content = output.subarray(offset, offset + length);
    offset += length + 1;
    return content;
  });
}

function assertRegularPath(root, relativePath) {
  let current = root;
  const parts = relativePath.split('/');
  for (const [index, part] of parts.entries()) {
    current = path.join(current, part);
    let item;
    try {
      item = lstatSync(current);
    } catch (error) {
      if (error.code === 'ENOENT') continue;
      throw error;
    }
    if (item.isSymbolicLink() || (index === parts.length - 1 ? !item.isFile() : !item.isDirectory())) {
      throw new Error(`Non-regular artifact destination: ${relativePath}`);
    }
  }
}

function assertOptions(options) {
  if (!/^[a-z][a-z0-9-]*$/.test(options.tool)) throw new Error('Invalid tool');
  if (!/^[A-Za-z][A-Za-z0-9]*$/.test(options.prefix)) throw new Error('Invalid prefix');
  if (!/^src\/ModularPipelines\.[A-Za-z0-9.]+$/.test(options.packageDirectory)) throw new Error('Invalid package directory');
  if (!/^[a-f0-9]{40}$/.test(options.sourceSha) || !/^[1-9][0-9]*$/.test(options.runId) ||
      !/^[1-9][0-9]*$/.test(options.runAttempt)) throw new Error('Invalid generation identity');
  if (!Number.isSafeInteger(options.maximumFileBytes) || options.maximumFileBytes < 1 || options.maximumFileBytes > 25 * 1024 * 1024) {
    throw new Error('Invalid file size limit');
  }
}

function validatePath(root, entry, options) {
  const { path: file, content } = entry;
  if (typeof file !== 'string' || !/^[A-Za-z0-9_./-]+$/.test(file) || file.split('/').some(part => !part || part === '.' || part === '..')) {
    throw new Error('Invalid artifact path');
  }
  const prefix = `${options.packageDirectory}/`;
  const generatedDocumentation = file === `docs/docs/mp-packages/cli/${options.tool}.md` && content !== null;
  if (!file.startsWith(prefix) && !generatedDocumentation) throw new Error(`Artifact path outside selected package: ${file}`);
  const relative = file.slice(prefix.length);
  const name = path.posix.basename(relative);
  const toolOwned = name.startsWith(options.prefix) || name.startsWith(`I${options.prefix}`);
  const generatedCode = /^(Options|Enums|Services|Extensions)\/[A-Za-z0-9_]+\.Generated\.cs$/.test(relative) && toolOwned;
  const generatedMetadata = relative === 'AssemblyInfo.Generated.cs' ||
    relative === `Generated/${options.prefix}.Generation.json` || relative === `Generated/${options.prefix}.CommandCoverage.json`;
  const legacyRoot = options.generateCommandFacade && [ `${options.prefix}.cs`, `I${options.prefix}.cs` ].includes(relative);
  const legacyCode = /^(Generated\/)?(Options|Enums|Services|Extensions)\/(?:[A-Za-z0-9_]+\/)*[A-Za-z0-9_]+\.cs$/.test(relative) && toolOwned;
  if (!generatedCode && !generatedMetadata && !generatedDocumentation && !(content === null && (legacyRoot || legacyCode))) {
    throw new Error(`Artifact path is not generated output for selected tool: ${file}`);
  }
  assertRegularPath(root, file);
}

export function validateArtifact(root, artifact, options) {
  assertOptions(options);
  if (artifact.formatVersion !== 1 || artifact.tool !== options.tool || artifact.sourceSha !== options.sourceSha ||
      artifact.runId !== options.runId || artifact.runAttempt !== options.runAttempt) {
    throw new Error('Artifact generation identity mismatch');
  }
  if (typeof artifact.coverageReport !== 'string' || Buffer.byteLength(artifact.coverageReport) > maximumReportBytes || artifact.coverageReport.includes('\0')) {
    throw new Error('Invalid coverage report');
  }
  if (!Array.isArray(artifact.changes) || artifact.changes.length > maximumFiles) throw new Error('Invalid artifact change list');
  const seen = new Map();
  let totalBytes = 0;
  const changes = artifact.changes.map(entry => {
    if (!entry || typeof entry !== 'object') throw new Error('Invalid artifact entry');
    validatePath(root, entry, options);
    const key = entry.path.toLowerCase();
    const previous = seen.get(key);
    // Case-only generated renames have one deletion and one replacement. Accept
    // that pair, but no duplicate writes or ambiguous aliases on Windows.
    if (previous && (previous.path === entry.path || (previous.content === null) === (entry.content === null) || previous.paired)) {
      throw new Error(`Duplicate artifact path: ${entry.path}`);
    }
    seen.set(key, { ...entry, paired: Boolean(previous) });
    if (entry.content === null) return { path: entry.path, bytes: null };
    if (typeof entry.content !== 'string' || entry.content.length > Math.ceil(options.maximumFileBytes / 3) * 4) {
      throw new Error(`Invalid or oversized artifact content: ${entry.path}`);
    }
    const bytes = Buffer.from(entry.content, 'base64');
    if (bytes.toString('base64') !== entry.content) throw new Error(`Invalid artifact encoding: ${entry.path}`);
    totalBytes += bytes.length;
    if (bytes.length > options.maximumFileBytes || totalBytes > maximumContentBytes) throw new Error('Artifact content exceeds size limit');
    const text = new TextDecoder('utf-8', { fatal: true }).decode(bytes);
    if (text.includes('\0')) throw new Error(`Binary artifact content: ${entry.path}`);
    return { path: entry.path, bytes };
  });
  const deletions = changes.filter(change => change.bytes === null);
  const originals = readBlobs(root, deletions.map(change => `HEAD:${change.path}`));
  deletions.forEach((change, index) => {
    const relative = change.path.slice(options.packageDirectory.length + 1);
    const legacyRoot = options.generateCommandFacade && [`${options.prefix}.cs`, `I${options.prefix}.cs`].includes(relative);
    if (!legacyRoot && !originals[index].toString('utf8').includes('<auto-generated')) {
      throw new Error(`Deletion lacks generated header: ${change.path}`);
    }
  });
  return changes;
}

export function exportArtifact(root, directory, logFile, options) {
  assertOptions(options);
  if (git(root, ['rev-parse', 'HEAD']).toString().trim() !== options.sourceSha) throw new Error('Checkout differs from generation source');
  const raw = git(root, ['diff', '--cached', '--no-renames', '--raw', '--no-abbrev', '-z']).toString().split('\0');
  const files = [];
  for (let index = 0; index < raw.length - 1; index += 2) {
    const [, mode, , sha] = raw[index].slice(1).split(' ');
    if (!['000000', '100644'].includes(mode)) throw new Error(`Non-regular staged file: ${raw[index + 1]}`);
    files.push({ path: raw[index + 1], sha: mode === '000000' ? null : sha });
  }
  // One batch keeps large AWS/Gcloud refreshes from spawning a process per file.
  const blobs = readBlobs(root, files.filter(file => file.sha !== null).map(file => file.sha));
  let blobIndex = 0;
  const changes = options.hasChanges === false ? [] : files.map(file => ({
    path: file.path, content: file.sha === null ? null : blobs[blobIndex++].toString('base64'),
  }));
  const log = readFileSync(logFile, 'utf8');
  const marker = log.lastIndexOf('Command coverage report:');
  const artifact = { formatVersion: 1, tool: options.tool, sourceSha: options.sourceSha, runId: options.runId, runAttempt: options.runAttempt,
    coverageReport: marker < 0 ? '' : log.slice(marker), changes };
  validateArtifact(root, artifact, options);
  const json = JSON.stringify(artifact);
  if (Buffer.byteLength(json) > maximumArtifactBytes) throw new Error('Artifact exceeds size limit');
  mkdirSync(directory, { recursive: true });
  writeFileSync(path.join(directory, artifactName), json);
}

export function importArtifact(root, directory, options) {
  assertOptions(options);
  if (git(root, ['rev-parse', 'HEAD']).toString().trim() !== options.sourceSha) throw new Error('Checkout differs from generation source');
  if (git(root, ['status', '--porcelain', '--untracked-files=all']).length) throw new Error('Publication checkout must be clean');
  const entries = readdirSync(directory);
  if (entries.length !== 1 || entries[0] !== artifactName) throw new Error('Unexpected artifact files');
  const artifactPath = path.join(directory, artifactName);
  assertRegularPath(directory, artifactName);
  if (lstatSync(artifactPath).size > maximumArtifactBytes) throw new Error('Artifact exceeds size limit');
  const artifact = JSON.parse(readFileSync(artifactPath, 'utf8'));
  const changes = validateArtifact(root, artifact, options);
  // Validate the entire handoff before mutating the checkout. Content is data only:
  // no archive extraction, patch application, scripts, builds, or package restore.
  const orderedChanges = [...changes.filter(change => change.bytes === null), ...changes.filter(change => change.bytes !== null)];
  for (const change of orderedChanges) {
    const destination = path.join(root, change.path);
    if (change.bytes === null) rmSync(destination);
    else {
      mkdirSync(path.dirname(destination), { recursive: true });
      writeFileSync(destination, change.bytes);
    }
  }
  return artifact;
}

if (process.argv[1] && import.meta.url === pathToFileURL(path.resolve(process.argv[1])).href) {
  const options = {
    tool: process.env.GENERATION_TOOL,
    prefix: process.env.GENERATION_PREFIX,
    packageDirectory: process.env.GENERATION_PACKAGE_DIRECTORY,
    generateCommandFacade: process.env.GENERATION_COMMAND_FACADE === 'true',
    sourceSha: process.env.GENERATION_SOURCE_SHA,
    runId: process.env.GITHUB_RUN_ID,
    runAttempt: process.env.GENERATION_RUN_ATTEMPT || process.env.GITHUB_RUN_ATTEMPT,
    maximumFileBytes: Number(process.env.GENERATION_MAX_FILE_BYTES),
    hasChanges: process.env.GENERATION_HAS_CHANGES !== 'false',
  };
  const [operation, directory, logFile] = process.argv.slice(2);
  if (operation === 'select') {
    const jobs = JSON.parse(readFileSync(directory, 'utf8')).flatMap(page => page.jobs);
    const artifacts = JSON.parse(readFileSync(logFile, 'utf8')).flatMap(page => page.artifacts);
    const selected = selectPublicationArtifact(jobs, artifacts, options.tool);
    appendFileSync(process.env.GITHUB_OUTPUT, `id=${selected?.id ?? ''}\nrun_attempt=${selected?.runAttempt ?? ''}\n`);
  } else if (operation === 'export') exportArtifact(process.env.GITHUB_WORKSPACE, directory, logFile, options);
  else if (operation === 'import') {
    const artifact = importArtifact(process.env.GITHUB_WORKSPACE, directory, options);
    writeFileSync(process.env.GENERATION_MANIFEST, artifact.changes.map(change => change.path).join('\n'));
    const delimiter = randomUUID();
    appendFileSync(process.env.GITHUB_OUTPUT,
      `has_changes=${artifact.changes.length > 0}\ncoverage-report<<${delimiter}\n${artifact.coverageReport}\n${delimiter}\n`);
  } else throw new Error('Expected select, export or import');
}
