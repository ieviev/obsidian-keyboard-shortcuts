module Fs.Obsidian.SettingTab

open Fable.Core
open Fs.Obsidian
open Microsoft.FSharp.Reflection
open ObsidianBindings
open Fable.Core.JsInterop

let createSettingForProperty (plugin:ExtendedPlugin<PluginSettings>) (settingTab:SettingTab) (propName:string) =
    obsidian.Setting.Create(settingTab.containerEl)
        .setName(U2.Case1 propName)
        .addText(fun txt ->
            let property =
                typeof<PluginSettings>
                |> FSharpType.GetRecordFields
                |> Seq.find (fun f -> f.Name = propName)
            
            !!property.GetValue(plugin.settings) |> txt.setValue |> ignore
            
            txt.onChange(fun value ->
                plugin.settings <-
                    plugin.settings
                    |> PluginSettings.withDynamicProp propName value
                None
            )
            |> ignore
            None
        )
    |> ignore
    

let createSettingDisplay (plugin:ExtendedPlugin<PluginSettings>) (settingtab:PluginSettingTab) () =
    let containerEl = settingtab.containerEl
    containerEl.empty()
    containerEl.createEl( "h2",
        unbox U2.Case1 {| text = "Obsidian Keyboard Shortcuts" |}) |> ignore
    
    let fields = typeof<PluginSettings> |> FSharpType.GetRecordFields
    let buildSetting = createSettingForProperty plugin settingtab
    
    fields
    |> Array.map (fun f -> f.Name)
    |> Array.iter buildSetting
    
    None
    

    
let create (app:App) (plugin:ExtendedPlugin<PluginSettings>) =
    let settingtab = obsidian.PluginSettingTab.Create(app,plugin)
    settingtab?display <- (createSettingDisplay plugin settingtab)
    settingtab?hide <- (fun f ->
        printJson "saving settings"
        plugin.saveSettings(plugin.settings) |> ignore
        None    
    )
    settingtab
