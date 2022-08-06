import { printJson, Command_forEditorCheck, SuggestModal_create, SuggestModal_withGetSuggestions, SuggestModal_withRenderSuggestion, SuggestModal_withOnChooseSuggestion, Command_forMenu, Clipboard_write, Content_getCodeBlocks, Seq_skipSafe, Command_forEditor } from "./Helpers.js";
import { comparePrimitives, min, uncurry } from "./fable_modules/fable-library.3.7.17/Util.js";
import { reverse } from "./fable_modules/fable-library.3.7.17/Array.js";
import { substring, split } from "./fable_modules/fable-library.3.7.17/String.js";
import { where, map, tryFind, tryFindIndex } from "./fable_modules/fable-library.3.7.17/Seq.js";
import { match } from "./fable_modules/fable-library.3.7.17/RegExp.js";
import * as obsidian from "obsidian";
import { some, defaultArg, value as value_7 } from "./fable_modules/fable-library.3.7.17/Option.js";

export function doNone(f) {
    return void 0;
}

export function goToPrevHeading(plugin) {
    return Command_forEditor("goToPrevHeading", "Go to previous heading", uncurry(2, (editor) => {
        const cursor = editor.getCursor();
        const linesbefore = reverse(split(editor.getValue(), ["\n"], null, 0).slice(void 0, (~(~cursor.line)) + 1));
        const foundOpt = tryFindIndex((f) => (match(/^(#+) /gu, f) != null), Seq_skipSafe(1, linesbefore));
        if (foundOpt != null) {
            const moveby = foundOpt | 0;
            const newpos = ((~(~cursor.line)) - moveby) - 1;
            editor.setCursor(newpos);
            return doNone;
        }
        else {
            return doNone;
        }
    }));
}

export function goToNextHeading(plugin) {
    return Command_forEditor("goToNextHeading", "Go to next heading", uncurry(2, (editor) => {
        const cursor = editor.getCursor();
        const linesafter = split(editor.getValue(), ["\n"], null, 0).slice(~(~cursor.line), split(editor.getValue(), ["\n"], null, 0).length);
        const foundOpt = tryFindIndex((f) => (match(/^(#+) /gu, f) != null), Seq_skipSafe(1, linesafter));
        if (foundOpt != null) {
            const moveby = foundOpt | 0;
            const newpos = ((~(~cursor.line)) + moveby) + 1;
            editor.setCursor(newpos);
            return doNone;
        }
        else {
            return doNone;
        }
    }));
}

export function copyNextCodeBlock(plugin) {
    return Command_forEditor("copyNextCodeBlock", "Copy Next Code Block", uncurry(2, (edit) => {
        let arg_1, objectArg;
        const matchValue = Content_getCodeBlocks(plugin.app);
        if (matchValue != null) {
            const blocks = matchValue;
            const cursor = edit.getCursor();
            const _arg = tryFind((f_1) => (f_1.startLine > (~(~cursor.line))), blocks);
            if (_arg != null) {
                const v = _arg;
                (arg_1 = (`copied:
${substring(v.content, 0, min(comparePrimitives, v.content.length, 50))}`), (objectArg = obsidian.Notice, new objectArg(arg_1)));
                Clipboard_write(v.content);
            }
            else {
                new obsidian.Notice("could not find a code block");
            }
            return doNone;
        }
        else {
            return doNone;
        }
    }));
}

export function copyCodeBlock(plugin) {
    return Command_forMenu("copyCodeBlock", "Copy Code Block", () => {
        const codeblocks = Content_getCodeBlocks(plugin.app);
        if (codeblocks == null) {
            return void 0;
        }
        else {
            const codeblocks_1 = value_7(codeblocks);
            const modal = SuggestModal_withOnChooseSuggestion((f_3) => {
                let arg_2, objectArg;
                (arg_2 = (`copied:
${substring(f_3.content, 0, min(comparePrimitives, f_3.content.length, 50))}`), (objectArg = obsidian.Notice, new objectArg(arg_2)));
                Clipboard_write(f_3.content);
            }, SuggestModal_withRenderSuggestion((f_2, elem) => {
                elem.innerText = defaultArg(f_2.title, f_2.content);
            }, SuggestModal_withGetSuggestions((queryInput) => {
                const query = obsidian.prepareQuery(queryInput);
                const matches = map((tuple) => tuple[0], where((f_1) => (f_1[1] != null), map((f) => {
                    const text = defaultArg(f.title, f.content);
                    return [f, obsidian.fuzzySearch(query, text)];
                }, codeblocks_1)));
                return Array.from(matches);
            }, SuggestModal_create(plugin.app))));
            modal.open();
            return void 0;
        }
    });
}

export function createAdmonitionView(plugin) {
    return Command_forEditorCheck("createAdmonitionView", "Create Code Block With Title", (check, editor, view) => {
        printJson("edit called");
        let v;
        const arg = obsidian.MarkdownEditView;
        const objectArg = plugin.app.workspace;
        v = objectArg.getActiveViewOfType(arg);
        printJson("edit called");
        let result = "";
        let textElement = null;
        let modal;
        const arg_1 = plugin.app;
        const objectArg_1 = obsidian.Modal;
        modal = (new objectArg_1(arg_1));
        modal.contentEl.createEl("h1", some("What\u0027s your name?"));
        let typesettings;
        let f;
        const arg_2 = modal.contentEl;
        const objectArg_2 = obsidian.Setting;
        f = (new objectArg_2(arg_2));
        f.setName("name");
        f.addText((txt) => {
            txt.onChange((value_2) => {
                result = value_2;
                return void 0;
            });
            textElement = txt;
            return void 0;
        });
        typesettings = f;
        let f_1;
        const arg_3 = modal.contentEl;
        const objectArg_3 = obsidian.Setting;
        f_1 = (new objectArg_3(arg_3));
        f_1.addButton((btn) => {
            btn.setButtonText("submit").setCta().onClick((_arg) => {
                modal.close();
                editor.replaceSelection(result);
                return void 0;
            });
            return void 0;
        });
        modal.open();
        textElement.inputEl.focus();
    });
}

export function insertHeading4(plugin) {
    return Command_forEditor("insertHeading4", "Insert heading 4", uncurry(2, (edit) => {
        edit.replaceSelection("#### ");
        return doNone;
    }));
}

export function insertAdmonitionInfo(plugin) {
    return Command_forEditor("insertAdmonitionInfo", "Insert Info Admonition", uncurry(2, (edit) => {
        edit.replaceSelection("````ad-info\ntitle: \n````");
        const cursor = edit.getCursor();
        edit.setCursor(cursor.line - 1);
        return doNone;
    }));
}

export function insertCodeBlock(plugin) {
    return Command_forEditor("insertCodeBlock", "Insert Code Block", uncurry(2, (edit) => {
        edit.replaceSelection(`\`\`\`\`${plugin.settings.defaultCodeBlockLanguage}

\`\`\`\``);
        const cursor = edit.getCursor();
        edit.setCursor(cursor.line - 1);
        return doNone;
    }));
}

export function insertTest(plugin) {
    return Command_forEditor("testcomand", "qqqqqq", uncurry(2, (edit) => {
        edit.replaceSelection("````\nhello\n````");
        const cursor = edit.getCursor();
        edit.setCursor(cursor.line - 1);
        return doNone;
    }));
}

//# sourceMappingURL=Commands.js.map
