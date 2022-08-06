import { getEnumerator, curry, uncurry } from "./fable_modules/fable-library.3.7.17/Util.js";
import { join, split, printf, toConsole } from "./fable_modules/fable-library.3.7.17/String.js";
import * as obsidian from "obsidian";
import { Record } from "./fable_modules/fable-library.3.7.17/Types.js";
import { record_type, option_type, string_type, int32_type } from "./fable_modules/fable-library.3.7.17/Reflection.js";
import { bind, flatten, value as value_1, map } from "./fable_modules/fable-library.3.7.17/Option.js";
import { singleton, empty, enumerateWhile, append, enumerateUsing, delay, tryFind, map as map_1, toArray, where } from "./fable_modules/fable-library.3.7.17/Seq.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./fable_modules/Fable.Promise.3.1.3/Promise.fs.js";
import { promise } from "./fable_modules/Fable.Promise.3.1.3/PromiseImpl.fs.js";

function Command_defaultCommand() {
    let mid = "";
    let mname = "";
    let _cb = void 0;
    let _ccb = void 0;
    let _hotkeys = void 0;
    let _editorCallback = void 0;
    let _editorCheckCallback = void 0;
    return new (class {
        get callback() {
            return _cb;
        }
        set callback(v) {
            _cb = v;
        }
        get checkCallback() {
            return _ccb;
        }
        set checkCallback(v_1) {
            _ccb = v_1;
        }
        get editorCallback() {
            return uncurry(2, _editorCallback);
        }
        set editorCallback(v_2) {
            _editorCallback = curry(2, v_2);
        }
        get editorCheckCallback() {
            return uncurry(3, _editorCheckCallback);
        }
        set editorCheckCallback(v_3) {
            _editorCheckCallback = curry(3, v_3);
        }
        get hotkeys() {
            return _hotkeys;
        }
        set hotkeys(v_4) {
            _hotkeys = v_4;
        }
        get icon() {
            return void 0;
        }
        set icon(v_5) {
        }
        get id() {
            return mid;
        }
        set id(v_6) {
            mid = v_6;
        }
        get mobileOnly() {
            return void 0;
        }
        set mobileOnly(v_7) {
        }
        get name() {
            return mname;
        }
        set name(v_8) {
            mname = v_8;
        }
    }
    )();
}

export function Command_forMenu(id, name, callback) {
    const cmd = Command_defaultCommand();
    cmd.id = id;
    cmd.name = name;
    cmd.callback = callback;
    return cmd;
}

export function Command_forEditor(id, name, callback) {
    const cmd = Command_defaultCommand();
    cmd.id = id;
    cmd.name = name;
    cmd.editorCallback = callback;
    return cmd;
}

export function Command_forEditorCheck(id, name, callback) {
    const cmd = Command_defaultCommand();
    cmd.id = id;
    cmd.name = name;
    cmd.editorCheckCallback = callback;
    return cmd;
}

export function printJson(x) {
    const arg_1 = JSON.stringify(x);
    toConsole(printf("%s"))(arg_1);
}

export function SuggestModal_create(app) {
    return new obsidian.SuggestModal(app);
}

export function SuggestModal_withGetSuggestions(query, sm) {
    sm.getSuggestions = (query);
    return sm;
}

export function SuggestModal_withOnChooseSuggestion(fn, sm) {
    sm.onChooseSuggestion = ((f, eventargs) => {
        fn(f);
    });
    return sm;
}

export function SuggestModal_withRenderSuggestion(fn, sm) {
    sm.renderSuggestion = ((sugg, elem) => {
        fn(sugg, elem);
        return void 0;
    });
    return sm;
}

export function SuggestModal_withDefaultRenderSuggestion(sm) {
    sm.renderSuggestion = ((sugg, elem) => {
        elem.innerText = JSON.stringify(sugg);
        return elem;
    });
    return sm;
}

export class Content_CodeBlockContent extends Record {
    constructor(startLine, title, content) {
        super();
        this.startLine = (startLine | 0);
        this.title = title;
        this.content = content;
    }
}

export function Content_CodeBlockContent$reflection() {
    return record_type("Fs.Obsidian.Helpers.Content.CodeBlockContent", [], Content_CodeBlockContent, () => [["startLine", int32_type], ["title", option_type(string_type)], ["content", string_type]]);
}

function Content_om(fn, x) {
    return map(fn, x);
}

export function Content_getCodeBlocks(app) {
    let option, objectArg_1;
    let view;
    const arg = obsidian.MarkdownView;
    const objectArg = app.workspace;
    view = objectArg.getActiveViewOfType(arg);
    if (view == null) {
        return void 0;
    }
    else {
        const view_1 = value_1(view);
        const lines = split(view_1.getViewData(), ["\n"], null, 0);
        const codeBlockSections = flatten(map((f) => map((d) => where((d_1) => (d_1.type === "code"), d), f.sections), flatten((option = app.workspace.getActiveFile(), map((objectArg_1 = app.metadataCache, (arg_1) => objectArg_1.getFileCache(arg_1)), option)))));
        const codeBlockTexts = map((optionalCodeblockSections) => toArray(map_1((f_1) => {
            const startLine = ((~(~f_1.position.start.line)) + 1) | 0;
            const endLine = ((~(~f_1.position.end.line)) - 1) | 0;
            const blockContent = lines.slice(startLine, endLine + 1);
            return new Content_CodeBlockContent(startLine, map((f_3) => f_3.slice("title:".length, f_3.length).trim(), tryFind((f_2) => (f_2.indexOf("title:") === 0), blockContent)), join("\n", where((f_4) => (!(f_4.indexOf("title:") === 0)), blockContent)));
        }, optionalCodeblockSections)), codeBlockSections);
        return codeBlockTexts;
    }
}

export function Content_getHeadings(app) {
    let option, objectArg_1;
    let view;
    const arg = obsidian.MarkdownView;
    const objectArg = app.workspace;
    view = objectArg.getActiveViewOfType(arg);
    if (view == null) {
        return void 0;
    }
    else {
        const view_1 = value_1(view);
        const lines = split(view_1.getViewData(), ["\n"], null, 0);
        const codeBlockSections = bind((f) => map((d) => d.slice(), f.headings), (option = app.workspace.getActiveFile(), bind((objectArg_1 = app.metadataCache, (arg_1) => objectArg_1.getFileCache(arg_1)), option)));
        const codeBlockTexts = map((headingCaches) => toArray(map_1((f_1) => {
            const startLine = ((~(~f_1.position.start.line)) + 1) | 0;
            const endLine = ((~(~f_1.position.end.line)) - 1) | 0;
            const blockContent = lines.slice(startLine, endLine + 1);
            return new Content_CodeBlockContent(startLine, map((f_3) => f_3.slice("title:".length, f_3.length), tryFind((f_2) => (f_2.indexOf("title:") === 0), blockContent)), join("\n", where((f_4) => (!(f_4.indexOf("title:") === 0)), blockContent)));
        }, headingCaches)), codeBlockSections);
        return codeBlockTexts;
    }
}

export function Seq_skipSafe(num, source) {
    return delay(() => enumerateUsing(getEnumerator(source), (e) => {
        let idx = 0;
        let loop = true;
        return append(enumerateWhile(() => ((idx < num) && loop), delay(() => append((!e["System.Collections.IEnumerator.MoveNext"]()) ? ((loop = false, empty())) : empty(), delay(() => {
            idx = ((idx + 1) | 0);
            return empty();
        })))), delay(() => enumerateWhile(() => e["System.Collections.IEnumerator.MoveNext"](), delay(() => singleton(e["System.Collections.Generic.IEnumerator`1.get_Current"]())))));
    }));
}

export class PluginSettings extends Record {
    constructor(defaultCodeBlockLanguage) {
        super();
        this.defaultCodeBlockLanguage = defaultCodeBlockLanguage;
    }
}

export function PluginSettings$reflection() {
    return record_type("Fs.Obsidian.Helpers.PluginSettings", [], PluginSettings, () => [["defaultCodeBlockLanguage", string_type]]);
}

export function PluginSettings_get_Default() {
    return new PluginSettings("");
}

export function PluginSettingsModule_withDynamicProp(key, value, settings) {
    if (key === "defaultCodeBlockLanguage") {
        return new PluginSettings(value);
    }
    else {
        throw (new Error(`unknown property ${key}`));
    }
}

export function Clipboard_write(txt) {
    const matchValue = navigator.clipboard;
    if (matchValue != null) {
        const v = matchValue;
        return v.writeText(txt);
    }
    else {
        return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
            return Promise.resolve();
        }));
    }
}

//# sourceMappingURL=Helpers.js.map
