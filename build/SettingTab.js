import * as obsidian from "obsidian";
import { find } from "./fable_modules/fable-library.3.7.17/Seq.js";
import { getValue, getRecordElements, name } from "./fable_modules/fable-library.3.7.17/Reflection.js";
import { printJson, PluginSettingsModule_withDynamicProp, PluginSettings$reflection } from "./Helpers.js";
import { some } from "./fable_modules/fable-library.3.7.17/Option.js";
import { map } from "./fable_modules/fable-library.3.7.17/Array.js";

export function createSettingForProperty(plugin, settingTab, propName) {
    (new obsidian.Setting(settingTab.containerEl)).setName(propName).addText((txt) => {
        let arg_1;
        const property = find((f) => (name(f) === propName), getRecordElements(PluginSettings$reflection()));
        (arg_1 = getValue(property, plugin.settings), txt.setValue(arg_1));
        txt.onChange((value_1) => {
            plugin.settings = PluginSettingsModule_withDynamicProp(propName, value_1, plugin.settings);
            return void 0;
        });
        return void 0;
    });
}

export function createSettingDisplay(plugin, settingtab, unitVar) {
    const containerEl = settingtab.containerEl;
    containerEl.empty();
    containerEl.createEl("h2", some(((arg) => arg)({
        text: "Obsidian Keyboard Shortcuts",
    })));
    const fields = getRecordElements(PluginSettings$reflection());
    const buildSetting = (propName) => {
        createSettingForProperty(plugin, settingtab, propName);
    };
    const array_1 = map(name, fields);
    array_1.forEach(buildSetting);
    return void 0;
}

export function create(app, plugin) {
    const settingtab = new obsidian.PluginSettingTab(app, plugin);
    settingtab.display = (() => createSettingDisplay(plugin, settingtab, void 0));
    settingtab.hide = ((f) => {
        printJson("saving settings");
        plugin.saveSettings(plugin.settings);
        return void 0;
    });
    return settingtab;
}

//# sourceMappingURL=SettingTab.js.map
