import { FSharpRef } from "./fable_modules/fable-library.3.7.17/Types.js";
import { Plugin } from "obsidian";
import { class_type } from "./fable_modules/fable-library.3.7.17/Reflection.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./fable_modules/Fable.Promise.3.1.3/Promise.fs.js";
import { promise } from "./fable_modules/Fable.Promise.3.1.3/PromiseImpl.fs.js";
import { some, value as value_1 } from "./fable_modules/fable-library.3.7.17/Option.js";
import { printJson, PluginSettings_get_Default } from "./Helpers.js";
import { create } from "./SettingTab.js";
import { iterate } from "./fable_modules/fable-library.3.7.17/Seq.js";
import { insertAdmonitionInfo, insertHeading4, goToNextHeading, goToPrevHeading, insertCodeBlock, copyNextCodeBlock, copyCodeBlock } from "./Commands.js";

export class Plugin2 extends Plugin {
    constructor(app, manifest) {
        super(app, manifest);
        const instance = new FSharpRef(null);
        this.app = app;
        instance.contents = this;
        this.plugin = instance.contents;
        Plugin2__init(this);
        this["init@15"] = 1;
        this.plugin.onload = (() => {
            Plugin2__onload(this);
        });
    }
}

export function Plugin2$reflection() {
    return class_type("Main.Plugin2", void 0, Plugin2, class_type("Main.BasePlugin", void 0, Plugin));
}

export function Plugin2_$ctor_Z7F5F6E8A(app, manifest) {
    return new Plugin2(app, manifest);
}

function Plugin2__init(this$) {
    this$.plugin.loadSettings = ((_arg) => PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => (this$.plugin.loadData().then((_arg_1) => {
        const data = _arg_1;
        if (data != null) {
            const v = value_1(data);
            return PromiseBuilder__Delay_62FBFDE1(promise, () => {
                this$.plugin.settings = v;
                return Promise.resolve();
            }).catch((_arg_2) => {
                const e = _arg_2;
                this$.plugin.settings = PluginSettings_get_Default();
                return Promise.resolve();
            });
        }
        else {
            this$.plugin.settings = PluginSettings_get_Default();
            return Promise.resolve();
        }
    })))));
    this$.plugin.saveSettings = ((settings) => PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        this$.plugin.settings = settings;
        return this$.plugin.saveData(some(settings)).then(() => (Promise.resolve(undefined)));
    })));
}

function Plugin2__onload(this$) {
    printJson("aciq:Obsidian Keyboard Shortcuts loaded");
    this$.plugin.settings = this$.plugin.loadSettings();
    const arg = create(this$.app, this$.plugin);
    this$.plugin.addSettingTab(arg);
    iterate((cmd) => {
        let arg_1;
        (arg_1 = cmd(this$.plugin), this$.plugin.addCommand(arg_1));
    }, [copyCodeBlock, copyNextCodeBlock, insertCodeBlock, goToPrevHeading, goToNextHeading, insertHeading4, insertAdmonitionInfo]);
}

//# sourceMappingURL=Main.js.map
