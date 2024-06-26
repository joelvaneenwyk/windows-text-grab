using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Text_Grab.Models;
using Text_Grab.Services;
using Text_Grab.Views;
using Application = System.Windows.Application;

namespace Text_Grab.Utilities;

public static class NotifyIconUtilities
{
    public static void SetupNotifyIcon()
    {
        App app = (App)Application.Current;
        if (app.TextGrabIcon is not null
            || app.NumberOfRunningInstances > 1)
        {
            return;
        }

        NotifyIcon icon = new();
        icon.Text = "Text Grab";
        icon.Icon = new Icon(Application.GetResourceStream(new Uri("/TealSelect.ico", UriKind.Relative)).Stream);
        icon.Visible = true;

        ContextMenuStrip? contextMenu = new();

        ToolStripMenuItem? settingsItem = new("&Settings");
        settingsItem.Click += (s, e) => { SettingsWindow sw = new(); sw.Show(); };
        ToolStripMenuItem? editLastItem = new("&Edit Last Grab");
        editLastItem.Click += (s, e) => { Singleton<HistoryService>.Instance.GetLastHistoryAsGrabFrame(); };
        ToolStripMenuItem? quickSimpleLookupItem = new("&Quick Simple Lookup");
        quickSimpleLookupItem.Click += (s, e) => { QuickSimpleLookup qsl = new(); qsl.Show(); };
        ToolStripMenuItem? previousGrabRegion = new("&Grab Previous Region");
        previousGrabRegion.Click += async (s, e) => { await OcrUtilities.GetTextFromPreviousFullscreenRegion(); };
        ToolStripMenuItem? fullScreenGrabItem = new("&Fullscreen Grab");
        fullScreenGrabItem.Click += (s, e) => { WindowUtilities.LaunchFullScreenGrab(); };
        ToolStripMenuItem? grabFrameItem = new("&Grab Frame");
        grabFrameItem.Click += (s, e) => { GrabFrame gf = new(); gf.Show(); };
        ToolStripMenuItem? editTextWindowItem = new("&Edit Text Window");
        editTextWindowItem.Click += (s, e) => { EditTextWindow etw = new(); etw.Show(); };

        ToolStripMenuItem? exitItem = new("&Close");
        exitItem.Click += (s, e) => { Application.Current.Shutdown(); };

        contextMenu.Items.AddRange(
            new[] {
                fullScreenGrabItem,
                previousGrabRegion,
                grabFrameItem,
                editTextWindowItem,
                quickSimpleLookupItem,
                editLastItem,
                settingsItem,
                exitItem
            }
        );
        icon.ContextMenuStrip = contextMenu;

        icon.MouseClick += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
                App.DefaultLaunch();
        };

        icon.Disposed += trayIcon_Disposed;

        RegisterHotKeys(app);

        app.TextGrabIcon = icon;
    }

    public static void RegisterHotKeys(App app)
    {
        IEnumerable<ShortcutKeySet> shortcuts = ShortcutKeysUtilities.GetShortcutKeySetsFromSettings();

        foreach (ShortcutKeySet keySet in shortcuts)
            if (keySet.IsEnabled && HotKeyManager.RegisterHotKey(keySet) is { } id)
                app.HotKeyIds.Add(id);

        HotKeyManager.HotKeyPressed -= HotKeyManager_HotKeyPressed;
        HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;
    }

    public static void UnregisterHotkeys(App app)
    {
        HotKeyManager.HotKeyPressed -= HotKeyManager_HotKeyPressed;

        foreach (int hotKeyId in app.HotKeyIds)
            HotKeyManager.UnregisterHotKey(hotKeyId);
    }

    private static void trayIcon_Disposed(object? sender, EventArgs e)
    {
        App app = (App)Application.Current;

        UnregisterHotkeys(app);
    }

    private static void HotKeyManager_HotKeyPressed(object? sender, HotKeyEventArgs e)
    {
        if (!AppUtilities.TextGrabSettings.GlobalHotkeysEnabled)
            return;

        IEnumerable<ShortcutKeySet> shortcuts = ShortcutKeysUtilities.GetShortcutKeySetsFromSettings();

        ShortcutKeyActions pressedAction = ShortcutKeyActions.None;
        foreach (ShortcutKeySet keySet in shortcuts)
            if (keySet.Equals(e))
                pressedAction = keySet.Action;

        switch (pressedAction)
        {
            case ShortcutKeyActions.None:
                break;
            case ShortcutKeyActions.Settings:
                break;
            case ShortcutKeyActions.Fullscreen:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WindowUtilities.LaunchFullScreenGrab();
                });
                break;
            case ShortcutKeyActions.GrabFrame:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    GrabFrame gf = new();
                    gf.Show();
                });
                break;
            case ShortcutKeyActions.Lookup:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    QuickSimpleLookup qsl = new();
                    qsl.Show();
                });
                break;
            case ShortcutKeyActions.EditWindow:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EditTextWindow etw = new();
                    etw.Show();
                    etw.Activate();
                });
                break;
            case ShortcutKeyActions.PreviousRegionGrab:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OcrUtilities.GetCopyTextFromPreviousRegion();
                });
                break;
            case ShortcutKeyActions.PreviousEditWindow:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EditTextWindow etw = new();
                    etw.OpenMostRecentTextHistoryItem();
                    etw.Show();
                    etw.Activate();
                });
                break;
            case ShortcutKeyActions.PreviousGrabFrame:
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Singleton<HistoryService>.Instance.GetLastHistoryAsGrabFrame();
                });
                break;
        }
    }
}
