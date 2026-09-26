import { spawnSync } from 'node:child_process';
import { readFileSync, writeFileSync } from 'node:fs';
import { pathToFileURL } from 'node:url';

export function extractReview(rawExecution) {
  let messages;
  try {
    messages = JSON.parse(rawExecution);
  } catch {
    // Parse errors can quote sensitive transcript content. Never log that content.
    throw new Error('Claude execution output is not valid JSON.');
  }
  if (!Array.isArray(messages) || messages.filter(message => message?.type === 'result').length !== 1) {
    throw new Error('Claude execution must contain exactly one final result.');
  }
  const result = messages.at(-1);
  if (result?.type !== 'result' || result.subtype !== 'success' || result.is_error !== false
    || typeof result.result !== 'string' || result.result.trim().length === 0) {
    throw new Error('Claude execution did not finish with a successful text result.');
  }
  return result.result;
}

function hasExactKeys(value, keys) {
  return value !== null && typeof value === 'object' && !Array.isArray(value)
    && Object.keys(value).length === keys.length && keys.every(key => Object.hasOwn(value, key));
}

function parseReviewJson(rawReview) {
  // The action can wrap its final JSON in Markdown despite the prompt. Accept
  // only a complete envelope; never extract a JSON fragment from surrounding prose.
  const fencedJson = typeof rawReview === 'string'
    ? rawReview.trim().match(/^```(?:json)?[\t ]*\r?\n([\s\S]*?)\r?\n```$/i)?.[1] : undefined;
  try {
    return JSON.parse(fencedJson ?? rawReview);
  } catch {
    if (typeof rawReview === 'string' && rawReview.trimStart().startsWith('```')) {
      throw new Error('Claude returned an invalid Markdown-fenced JSON review.');
    }
    if (typeof rawReview === 'string' && rawReview.trimStart().startsWith('{')) {
      throw new Error('Claude returned malformed JSON object text.');
    }
    if (typeof rawReview === 'string' && /^\s*#{1,6}\s/.test(rawReview)) {
      throw new Error('Claude returned a Markdown review instead of JSON.');
    }
    if (typeof rawReview === 'string' && rawReview.trimStart().startsWith('<')) {
      throw new Error('Claude returned markup instead of a JSON review.');
    }
    throw new Error('Claude did not return a valid structured review.');
  }
}

function parseReview(rawReview, changedFiles) {
  const review = parseReviewJson(rawReview);
  const validText = (value, minimumLength = 1) => typeof value === 'string' && value.trim().length >= minimumLength;
  const notes = review?.notes;
  if (!hasExactKeys(review, ['summary', 'findings', 'notes', 'evidence'])) {
    throw new Error('A review requires exactly summary, findings, notes, and evidence fields.');
  }
  if (!validText(review.summary, 40) || !Array.isArray(review.findings)
    || !review.findings.every(finding => validText(finding, 20))
    || !Array.isArray(notes) || !notes.every(note => validText(note))) {
    throw new Error('A review needs a descriptive summary (40 characters), findings (20 characters each), and nonempty notes.');
  }

  if (!Array.isArray(changedFiles) || !changedFiles.every(path => validText(path))) {
    throw new Error('The workflow must supply the captured changed-file list.');
  }
  const evidence = review.evidence;
  if (!Array.isArray(evidence) || (changedFiles.length > 0 && evidence.length === 0)
    || !evidence.every(item => hasExactKeys(item, ['path', 'assessment'])
      && changedFiles.includes(item.path) && validText(item.assessment, 40))
    || new Set(evidence.map(item => item.path)).size !== evidence.length) {
    throw new Error('Review evidence must describe checks against distinct files in the captured diff.');
  }

  return review;
}

export function buildReview(rawReview, headSha, changedFiles) {
  if (!/^[a-f0-9]{40}$/.test(headSha ?? '')) {
    throw new Error('A captured pull request head SHA is required.');
  }

  const review = parseReview(rawReview, changedFiles);
  const { notes, evidence } = review;

  // Reviews may discuss the verdict format. Render model-supplied HTML comments
  // literally so only the publisher's footer can act as a machine-readable verdict.
  const render = text => text.trim().replaceAll('<!--', '&lt;!--');

  const verdict = review.findings.length === 0 ? 'CLEAR' : 'BLOCKING';
  const body = [
    '## Claude Review',
    render(review.summary),
    '### Review evidence',
    evidence.length === 0 ? 'The captured diff contains no changed files.'
      : evidence.map(item => `${render(item.path)}: ${render(item.assessment)}`).join('\n\n'),
    review.findings.length === 0 ? 'No actionable findings.' : review.findings.map(render).join('\n\n'),
    ...(notes.length === 0 ? [] : ['### Optional follow-up notes', notes.map(render).join('\n\n')]),
    `<!-- REVIEW_VERDICT: ${verdict} HEAD: ${headSha} -->`,
  ].join('\n\n');
  if (Buffer.byteLength(body, 'utf8') > 60000) {
    throw new Error('The review exceeds the comment size budget.');
  }
  return body;
}

export function runGitHub(args, input, spawn = spawnSync) {
  const result = spawn('gh', args, { input, encoding: 'utf8', shell: false });
  if (result.error || result.status !== 0) {
    const detail = (result.stderr || result.error?.message || 'No error details returned.').trim();
    throw new Error(`GitHub ${args[0]} ${args[1]} failed (exit ${result.status}): ${detail}`);
  }
  return result.stdout;
}

export function publishReview({ rawReview, headSha, prNumber, repository, changedFiles }, run = runGitHub) {
  if (!/^[1-9]\d*$/.test(prNumber ?? '')
    || !/^[A-Za-z0-9_.-]+\/[A-Za-z0-9_.-]+$/.test(repository ?? '')) {
    throw new Error('The workflow must supply a valid pull request and repository.');
  }
  const body = buildReview(rawReview, headSha, changedFiles);
  const verifyHead = () => {
    const current = JSON.parse(run([
      'pr', 'view', prNumber, '--repo', repository, '--json', 'headRefOid,state',
    ]));
    if (current.state !== 'OPEN' || current.headRefOid !== headSha) {
      throw new Error('The pull request is closed or its head changed during review.');
    }
  };
  verifyHead();
  // Keep model output out of shell commands and command-line arguments.
  // A formal review is visible to latestReviews in Assert-PrGreen.ps1. Bind it to
  // the reviewed commit so a head-change race cannot authorize a different head.
  run(['api', '--method', 'POST', `repos/${repository}/pulls/${prNumber}/reviews`, '--input', '-'],
    JSON.stringify({ commit_id: headSha, event: 'COMMENT', body }));
  verifyHead();
}

function retainReviewFixture(rawReview) {
  if (!process.env.REVIEW_FIXTURE_OUTPUT) {
    return;
  }
  try {
    // Retain only the successfully published final response, never tool messages,
    // credentials, session identifiers, or other private transcript metadata.
    writeFileSync(process.env.REVIEW_FIXTURE_OUTPUT, JSON.stringify([
      { type: 'result', subtype: 'success', is_error: false, result: rawReview },
    ], null, 2));
  } catch {
    console.warn('Optional review fixture could not be retained.');
  }
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  try {
    const rawReview = extractReview(readFileSync(process.env.REVIEW_EXECUTION_FILE, 'utf8'));
    publishReview({
      rawReview,
      headSha: process.env.REVIEW_HEAD_SHA,
      prNumber: process.env.PR_NUMBER,
      repository: process.env.GH_REPO,
      changedFiles: readFileSync(process.env.REVIEW_CHANGED_FILES, 'utf8').split('\0').filter(Boolean),
    });
    retainReviewFixture(rawReview);
  } catch (error) {
    console.error(error.message);
    process.exitCode = 1;
  }
}
