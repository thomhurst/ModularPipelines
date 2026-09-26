import { readFileSync, realpathSync } from 'node:fs';
import { isAbsolute, relative, resolve, sep } from 'node:path';
import { pathToFileURL } from 'node:url';

// Never print transcript content: even error messages can contain credentials.
export function classifyFailure(records) {
  const errors = records.filter(record => record?.type === 'result' && record.is_error === true
    || record?.type === 'assistant' && record.isApiErrorMessage === true);
  const text = JSON.stringify(errors);
  if (/usage limit|quota|rate.?limit|hit your limit|too many requests/i.test(text)) return 'usage-or-rate-limit';
  if (/credit balance|billing|payment|insufficient credits/i.test(text)) return 'billing';
  if (/unauthorized|authentication|oauth|invalid.api.key|token.{0,30}expired|not logged in/i.test(text)) return 'authentication';
  if (/model.{0,60}(not found|unavailable|not supported)|invalid model/i.test(text)) return 'model-unavailable';
  if (/overloaded|connection|network|timeout|service unavailable/i.test(text)) return 'service-or-network';
  if (/structured.output|json.schema/i.test(text)) return 'structured-output';
  return errors.length ? 'unclassified-error' : 'no-error-record';
}

export function diagnose(file, runnerTemp) {
  try {
    const root = realpathSync(runnerTemp);
    const target = realpathSync(file || resolve(root, 'claude-execution-output.json'));
    const child = relative(root, target);
    if (!child || child === '..' || child.startsWith(`..${sep}`) || isAbsolute(child)) return 'invalid-output-path';
    const content = readFileSync(target, 'utf8');
    let records;
    try {
      const parsed = JSON.parse(content);
      records = Array.isArray(parsed) ? parsed : [parsed];
    } catch {
      records = content.split(/\r?\n/).filter(line => line.trim()).map(line => JSON.parse(line));
    }
    return classifyFailure(records);
  } catch {
    return 'output-unavailable';
  }
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
  console.log(`Claude failure category: ${diagnose(process.env.CLAUDE_EXECUTION_FILE, process.env.RUNNER_TEMP)}`);
}
