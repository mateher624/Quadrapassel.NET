//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace Quadrapassel
//{
//    public class Quadrapassel : Gtk.Application
//    {
//        /* Application settings */
//        private Settings _settings;

//        /* Main window */
//        private Gtk.Window _window;
//        private Gtk.MenuButton _menuButton;
//        private int _windowWidth;
//        private int _windowHeight;
//        private bool _isMaximized;
//        private bool _isTiled;

//        /* Game being played */
//        private Game _game;

//        /* Rendering of game */
//        private GameView _view;

//        /* Preview of the next shape */
//        private Preview _preview;

//        /* Label showing current score */
//        private Gtk.Label _scoreLabel;

//        /* Label showing the number of lines destroyed */
//        private Gtk.Label _nDestroyedLabel;

//        /* Label showing the current level */
//        private Gtk.Label _levelLabel;

//        private History _history;

//        private SimpleAction _pauseAction;

//        private Gtk.Button _pausePlayButton;
//        private Gtk.Image _pausePlayButtonImage;

//        private Gtk.Dialog _preferencesDialog;
//        private Gtk.SpinButton _startingLevelSpin;
//        private Preview _themePreview;
//        private Gtk.SpinButton _fillHeightSpinner;
//        private Gtk.SpinButton _fillProbSpinner;
//        private Gtk.CheckButton _doPreviewToggle;
//        private Gtk.CheckButton _difficultBlocksToggle;
//        private Gtk.CheckButton _rotateCounterClockWiseToggle;
//        private Gtk.CheckButton _showShadowToggle;
//        private Gtk.CheckButton _soundToggle;
//        private Gtk.ListStore _controlsModel;

//        private Manette.Monitor _manetteMonitor;

//        private readonly GLib.ActionEntry[] _actionEntries =
//        {
//            { "new-game",      NewGameCb    },
//            { "pause",         PauseCb       },
//            { "scores",        ScoresCb      },
//            { "menu",          MenuCb },
//            { "preferences",   PreferencesCb },
//            { "help",          HelpCb        },
//            { "about",         AboutCb       },
//            { "quit",          QuitCb        }
//        };

//        public Quadrapassel()
//        {
//            Object(application_id: "org.gnome.Quadrapassel", flags: ApplicationFlags.FLAGS_NONE);
//        }

//        protected override void Startup()
//        {
//            base.startup();

//            Gtk.Settings.get_default().set("gtk-application-prefer-dark-theme", true);

//            add_action_entries(_actionEntries, this);
//            set_accels_for_action("app.new-game", { "<Primary>n"});
//            set_accels_for_action("app.pause", { "Pause"});
//            set_accels_for_action("app.menu", { "F10"});
//            set_accels_for_action("app.help", { "F1"});
//            set_accels_for_action("app.quit", { "<Primary>q"});
//            _pauseAction = lookup_action("pause") as SimpleAction;

//            _settings = new Settings("org.gnome.Quadrapassel");

//            _window = new Gtk.ApplicationWindow(this);
//            _window.icon_name = "org.gnome.Quadrapassel";
//            _window.set_events(_window.get_events() | Gdk.EventMask.KEY_PRESS_MASK | Gdk.EventMask.KEY_RELEASE_MASK);
//            _window.title = _("Quadrapassel");
//            _window.size_allocate.connect(SizeAllocateCb);
//            _window.window_state_event.connect(WindowStateEventCb);
//            _window.key_press_event.connect(KeyPressEventCb);
//            _window.key_release_event.connect(KeyReleaseEventCb);
//            _window.set_default_size(_settings.get_int("window-width"), _settings.get_int("window-height"));
//            if (_settings.get_boolean("window-is-maximized"))
//                _window.maximize();

//            var headerbar = new Gtk.HeaderBar();
//            headerbar.show_close_button = true;
//            headerbar.set_title(_("Quadrapassel"));
//            headerbar.show();
//            _window.set_titlebar(headerbar);

//            var menu = new Menu();
//            var section = new Menu();
//            menu.append_section(null, section);
//            section.append(_("_New Game"), "app.new-game");
//            section.append(_("_Scores"), "app.scores");
//            section = new Menu();
//            menu.append_section(null, section);
//            section.append(_("_Preferences"), "app.preferences");
//            section.append(_("_Help"), "app.help");
//            section.append(_("_About Quadrapassel"), "app.about");
//            _menuButton = new Gtk.MenuButton();
//            _menuButton.set_image(new Gtk.Image.from_icon_name("open-menu-symbolic", Gtk.IconSize.BUTTON));
//            _menuButton.show();
//            _menuButton.set_menu_model(menu);

//            headerbar.pack_end(_menuButton);

//            var game_grid = new Gtk.Grid();
//            game_grid.set_column_homogeneous(true);
//            _window.add(game_grid);

//            _view = new GameView();
//            _view.theme = _settings.get_string("theme");
//            _view.mute = !_settings.get_boolean("sound");
//            _view.show_shadow = _settings.get_boolean("show-shadow");
//            _view.game = new Game(20, 14, 1, 20, 10);
//            _view.show();
//            var game_aspect = new Gtk.AspectFrame(null, 0.5f, 0.5f, 14.0f / 20.0f, false);
//            game_aspect.show();
//            game_aspect.add(_view);
//            game_aspect.border_width = 12;
//            game_grid.attach(game_aspect, 0, 1, 2, 17);

//            _pausePlayButton = new Gtk.Button();
//            _pausePlayButtonImage = new Gtk.Image.from_icon_name("media-playback-start-symbolic", Gtk.IconSize.DIALOG);
//            _pausePlayButton.add(_pausePlayButtonImage);
//            _pausePlayButton.action_name = "app.new-game";
//            _pausePlayButton.tooltip_text = _("Start a new game");
//            _pausePlayButton.margin = 30;
//            _pausePlayButtonImage.show();
//            _pausePlayButton.show();

//            var preview_frame = new Gtk.AspectFrame(_("Next"), 0.5f, 0.5f, 1.0f, false);
//            preview_frame.set_label_align(0.5f, 1.0f);
//            _preview = new Preview(preview_frame);
//            _preview.theme = _settings.get_string("theme");
//            _preview.enabled = _settings.get_boolean("do-preview");
//            preview_frame.add(_preview);
//            preview_frame.show();
//            _preview.show();

//            game_grid.attach(preview_frame, 2, 1, 1, 3);
//            game_grid.show();

//            var label = new Gtk.Label(null);
//            label.set_markup("<span color='gray'>%s</span>".printf(_("Score")));
//            label.set_alignment(0.5f, 0.5f);
//            label.show();
//            game_grid.attach(label, 2, 5, 1, 1);
//            _scoreLabel = new Gtk.Label("<big>-</big>");
//            _scoreLabel.set_use_markup(true);
//            _scoreLabel.set_alignment(0.5f, 0.0f);
//            _scoreLabel.show();
//            game_grid.attach(_scoreLabel, 2, 6, 1, 2);

//            label = new Gtk.Label(null);
//            label.set_markup("<span color='gray'>%s</span>".printf(_("Lines")));
//            label.set_alignment(0.5f, 0.5f);
//            label.show();
//            game_grid.attach(label, 2, 9, 1, 1);
//            _nDestroyedLabel = new Gtk.Label("<big>-</big>");
//            _nDestroyedLabel.set_use_markup(true);
//            _nDestroyedLabel.set_alignment(0.5f, 0.0f);
//            _nDestroyedLabel.show();
//            game_grid.attach(_nDestroyedLabel, 2, 10, 1, 2);

//            label = new Gtk.Label(null);
//            label.set_markup("<span color='gray'>%s</span>".printf(_("Level")));
//            label.set_alignment(0.5f, 0.5f);
//            label.show();
//            game_grid.attach(label, 2, 13, 1, 1);
//            _levelLabel = new Gtk.Label("<big>-</big>");
//            _levelLabel.set_use_markup(true);
//            _levelLabel.set_alignment(0.5f, 0.0f);
//            _levelLabel.show();
//            game_grid.attach(_levelLabel, 2, 14, 1, 2);

//            game_grid.attach(_pausePlayButton, 2, 16, 1, 2);

//            _manetteMonitor = new Manette.Monitor();
//            _manetteMonitor.device_connected.connect(ManetteDeviceConnectedCb);
//            var manette_iterator = _manetteMonitor.iterate();
//            Manette.Device manette_device = null;
//            while (manette_iterator.next(out manette_device))
//                ManetteDeviceConnectedCb(manette_device);

//            _history = new History(Path.build_filename(Environment.get_user_data_dir(), "quadrapassel", "history"));
//            _history.load();

//            _pauseAction.set_enabled(false);
//        }

//        private void SizeAllocateCb(Gtk.Allocation allocation)
//        {
//            if (_isMaximized || _isTiled)
//                return;
//            _window.get_size(out _windowWidth, out _windowHeight);
//        }

//        private bool WindowStateEventCb(Gdk.EventWindowState event_b)
//        {
//            if ((event_b.changed_mask & Gdk.WindowState.MAXIMIZED) != 0)
//                _isMaximized = (event_b.new_window_state & Gdk.WindowState.MAXIMIZED) != 0;
//            /* We don’t save this state, but track it for saving size allocation */
//            if ((event_b.changed_mask & Gdk.WindowState.TILED) != 0)
//                _isTiled = (event_b.new_window_state & Gdk.WindowState.TILED) != 0;
//            return false;
//        }

//        protected override void Shutdown()
//        {
//            base.shutdown();

//            /* Save window state */
//            _settings.set_int("window-width", _windowWidth);
//            _settings.set_int("window-height", _windowHeight);
//            _settings.set_boolean("window-is-maximized", _isMaximized);

//            /* Record the score if the game isn't over. */
//            if (_game != null && !_game.game_over && _game.score > 0)
//            {
//                var date = new DateTime.now_local();
//                var entry = new HistoryEntry(date, _game.score);
//                _history.add(entry);
//                _history.save();
//            }
//        }

//        protected override void Activate()
//        {
//            _window.present();
//        }

//        private void PreferencesDialogCloseCb()
//        {
//            _preferencesDialog.destroy();
//            _preferencesDialog = null;
//        }

//        private void PreferencesDialogResponseCb(int response_id)
//        {
//            PreferencesDialogCloseCb();
//        }

//        private void PreferencesCb()
//        {
//            if (_preferencesDialog != null)
//            {
//                _preferencesDialog.present();
//                return;
//            }

//            _preferencesDialog = new Gtk.Dialog.with_buttons(_("Preferences"),
//                                                              _window,
//                                                              Gtk.DialogFlags.USE_HEADER_BAR,
//                                                              null);
//            _preferencesDialog.set_border_width(5);
//            var vbox = (Gtk.Box)_preferencesDialog.get_content_area();
//            vbox.set_spacing(2);
//            _preferencesDialog.close.connect(PreferencesDialogCloseCb);
//            _preferencesDialog.response.connect(PreferencesDialogResponseCb);

//            var notebook = new Gtk.Notebook();
//            notebook.set_border_width(5);
//            vbox.pack_start(notebook, true, true, 0);

//            var grid = new Gtk.Grid();
//            grid.set_row_spacing(6);
//            grid.set_column_spacing(12);
//            grid.border_width = 12;
//            var label = new Gtk.Label(_("Game"));
//            notebook.append_page(grid, label);

//            /* pre-filled rows */
//            label = new Gtk.Label.with_mnemonic(_("_Number of pre-filled rows:"));
//            label.set_alignment(0, 0.5f);
//            label.set_hexpand(true);
//            grid.attach(label, 0, 0, 1, 1);

//            var adj = new Gtk.Adjustment(_settings.get_int("line-fill-height"), 0, 15, 1, 5, 0);
//            // the maximum should be at least 4 less than the new game height but as long as the game height is a magic 20 and not a setting, we can keep it at 15
//            _fillHeightSpinner = new Gtk.SpinButton(adj, 10, 0);
//            _fillHeightSpinner.set_update_policy(Gtk.SpinButtonUpdatePolicy.ALWAYS);
//            _fillHeightSpinner.set_snap_to_ticks(true);
//            _fillHeightSpinner.value_changed.connect(FillHeightSpinnerValueChangedCb);
//            grid.attach(_fillHeightSpinner, 1, 0, 1, 1);
//            label.set_mnemonic_widget(_fillHeightSpinner);

//            /* pre-filled rows density */
//            label = new Gtk.Label.with_mnemonic(_("_Density of blocks in a pre-filled row:"));
//            label.set_alignment(0, 0.5f);
//            label.set_hexpand(true);
//            grid.attach(label, 0, 1, 1, 1);

//            adj = new Gtk.Adjustment(_settings.get_int("line-fill-probability"), 0, 10, 1, 5, 0);
//            _fillProbSpinner = new Gtk.SpinButton(adj, 10, 0);
//            _fillProbSpinner.set_update_policy(Gtk.SpinButtonUpdatePolicy.ALWAYS);
//            _fillProbSpinner.set_snap_to_ticks(true);
//            _fillProbSpinner.value_changed.connect(FillProbSpinnerValueChangedCb);
//            grid.attach(_fillProbSpinner, 1, 1, 1, 1);
//            label.set_mnemonic_widget(_fillProbSpinner);

//            /* starting level */
//            label = new Gtk.Label.with_mnemonic(_("_Starting level:"));
//            label.set_alignment(0, 0.5f);
//            label.set_hexpand(true);
//            grid.attach(label, 0, 2, 1, 1);

//            adj = new Gtk.Adjustment(_settings.get_int("starting-level"), 1, 20, 1, 5, 0);
//            _startingLevelSpin = new Gtk.SpinButton(adj, 10.0, 0);
//            _startingLevelSpin.set_update_policy(Gtk.SpinButtonUpdatePolicy.ALWAYS);
//            _startingLevelSpin.set_snap_to_ticks(true);
//            _startingLevelSpin.value_changed.connect(StartingLevelValueChangedCb);
//            grid.attach(_startingLevelSpin, 1, 2, 1, 1);
//            label.set_mnemonic_widget(_startingLevelSpin);

//            _soundToggle = new Gtk.CheckButton.with_mnemonic(_("_Enable sounds"));
//            _soundToggle.set_active(_settings.get_boolean("sound"));
//            _soundToggle.toggled.connect(SoundToggleToggledCb);
//            grid.attach(_soundToggle, 0, 3, 2, 1);

//            _difficultBlocksToggle = new Gtk.CheckButton.with_mnemonic(_("Choose difficult _blocks"));
//            _difficultBlocksToggle.set_active(_settings.get_boolean("pick-difficult-blocks"));
//            _difficultBlocksToggle.toggled.connect(DifficultBlocksToggledCb);
//            grid.attach(_difficultBlocksToggle, 0, 4, 2, 1);

//            _doPreviewToggle = new Gtk.CheckButton.with_mnemonic(_("_Preview next block"));
//            _doPreviewToggle.set_active(_settings.get_boolean("do-preview"));
//            _doPreviewToggle.toggled.connect(DoPreviewToggleToggledCb);
//            grid.attach(_doPreviewToggle, 0, 5, 2, 1);

//            /* rotate counter clock wise */
//            _rotateCounterClockWiseToggle = new Gtk.CheckButton.with_mnemonic(_("_Rotate blocks counterclockwise"));
//            _rotateCounterClockWiseToggle.set_active(_settings.get_boolean("rotate-counter-clock-wise"));
//            _rotateCounterClockWiseToggle.toggled.connect(SetRotateCounterClockWise);
//            grid.attach(_rotateCounterClockWiseToggle, 0, 6, 2, 1);

//            _showShadowToggle = new Gtk.CheckButton.with_mnemonic(_("Show _where the block will land"));
//            _showShadowToggle.set_active(_settings.get_boolean("show-shadow"));
//            _showShadowToggle.toggled.connect(UserTargetToggledCb);
//            grid.attach(_showShadowToggle, 0, 7, 2, 1);

//            /* controls page */
//            _controlsModel = new Gtk.ListStore(4, typeof(string), typeof(string), typeof(uint), typeof(uint));
//            Gtk.TreeIter iter;
//            _controlsModel.append(out iter);
//            var keyval = _settings.get_int("key-left");
//            _controlsModel.set(iter, 0, "key-left", 1, _("Move left"), 2, keyval);
//            _controlsModel.append(out iter);
//            keyval = _settings.get_int("key-right");
//            _controlsModel.set(iter, 0, "key-right", 1, _("Move right"), 2, keyval);
//            _controlsModel.append(out iter);
//            keyval = _settings.get_int("key-down");
//            _controlsModel.set(iter, 0, "key-down", 1, _("Move down"), 2, keyval);
//            _controlsModel.append(out iter);
//            keyval = _settings.get_int("key-drop");
//            _controlsModel.set(iter, 0, "key-drop", 1, _("Drop"), 2, keyval);
//            _controlsModel.append(out iter);
//            keyval = _settings.get_int("key-rotate");
//            _controlsModel.set(iter, 0, "key-rotate", 1, _("Rotate"), 2, keyval);
//            _controlsModel.append(out iter);
//            keyval = _settings.get_int("key-pause");
//            _controlsModel.set(iter, 0, "key-pause", 1, _("Pause"), 2, keyval);
//            var controls_view = new Gtk.TreeView.with_model(_controlsModel);
//            controls_view.headers_visible = false;
//            controls_view.enable_search = false;
//            var label_renderer = new Gtk.CellRendererText();
//            controls_view.insert_column_with_attributes(-1, "Control", label_renderer, "text", 1);
//            var key_renderer = new Gtk.CellRendererAccel();
//            key_renderer.editable = true;
//            key_renderer.accel_mode = Gtk.CellRendererAccelMode.OTHER;
//            key_renderer.accel_edited.connect(AccelEditedCb);
//            key_renderer.accel_cleared.connect(AccelClearedCb);
//            controls_view.insert_column_with_attributes(-1, "Key", key_renderer, "accel-key", 2, "accel-mods", 3);

//            var controls_list = new Gtk.ScrolledWindow(null, null);
//            controls_list.border_width = 12;
//            controls_list.hscrollbar_policy = Gtk.PolicyType.NEVER;
//            controls_list.vscrollbar_policy = Gtk.PolicyType.ALWAYS;
//            controls_list.shadow_type = Gtk.ShadowType.IN;
//            controls_list.add(controls_view);
//            label = new Gtk.Label(_("Controls"));
//            notebook.append_page(controls_list, label);

//            /* theme page */
//            vbox = new Gtk.Box(Gtk.Orientation.VERTICAL, 0);
//            vbox.set_border_width(12);
//            label = new Gtk.Label(_("Theme"));
//            notebook.append_page(vbox, label);

//            var theme_combo = new Gtk.ComboBox();
//            vbox.pack_start(theme_combo, false, true, 0);
//            var theme_store = new Gtk.ListStore(2, typeof(string), typeof(string));
//            theme_combo.model = theme_store;
//            var renderer = new Gtk.CellRendererText();
//            theme_combo.pack_start(renderer, true);
//            theme_combo.add_attribute(renderer, "text", 0);

//            theme_store.append(out iter);
//            theme_store.set(iter, 0, _("Plain"), 1, "plain", -1);
//            if (_settings.get_string("theme") == "plain")
//                theme_combo.set_active_iter(iter);

//            theme_store.append(out iter);
//            theme_store.set(iter, 0, _("Tango Flat"), 1, "tangoflat", -1);
//            if (_settings.get_string("theme") == "tangoflat")
//                theme_combo.set_active_iter(iter);

//            theme_store.append(out iter);
//            theme_store.set(iter, 0, _("Tango Shaded"), 1, "tangoshaded", -1);
//            if (_settings.get_string("theme") == "tangoshaded")
//                theme_combo.set_active_iter(iter);

//            theme_store.append(out iter);
//            theme_store.set(iter, 0, _("Clean"), 1, "clean", -1);
//            if (_settings.get_string("theme") == "clean")
//                theme_combo.set_active_iter(iter);

//            theme_combo.changed.connect(ThemeComboChangedCb);

//            _themePreview = new Preview(null);
//            _themePreview.game = new Game();
//            _themePreview.theme = _settings.get_string("theme");
//            vbox.pack_start(_themePreview, true, true, 0);

//            _preferencesDialog.show_all();
//        }

//        private void SoundToggleToggledCb()
//        {
//            var play_sound = _soundToggle.get_active();
//            _settings.set_boolean("sound", play_sound);
//            _view.mute = !play_sound;
//        }

//        private void DoPreviewToggleToggledCb()
//        {
//            var preview_enabled = _doPreviewToggle.get_active();
//            _settings.set_boolean("do-preview", preview_enabled);
//            _preview.enabled = preview_enabled;
//        }

//        private void DifficultBlocksToggledCb()
//        {
//            _settings.set_boolean("pick-difficult-blocks", _difficultBlocksToggle.get_active());
//        }

//        private void SetRotateCounterClockWise()
//        {
//            _settings.set_boolean("rotate-counter-clock-wise", _rotateCounterClockWiseToggle.get_active());
//        }

//        private void UserTargetToggledCb()
//        {
//            var show_shadow = _showShadowToggle.get_active();
//            _settings.set_boolean("show-shadow", show_shadow);
//            _view.show_shadow = show_shadow;
//        }

//        private void AccelEditedCb(Gtk.CellRendererAccel cell, string path_string, uint keyval, Gdk.ModifierType mask, uint hardware_keycode)
//        {
//            var path = new Gtk.TreePath.from_string(path_string);
//            if (path == null)
//                return;

//            Gtk.TreeIter iter;
//            if (!_controlsModel.get_iter(out iter, path))
//                return;

//            string? key = null;
//            _controlsModel.get(iter, 0, out key);
//            if (key == null)
//                return;

//            _controlsModel.set(iter, 2, keyval);
//            _settings.set_int(key, (int)keyval);
//        }

//        private void AccelClearedCb(Gtk.CellRendererAccel cell, string path_string)
//        {
//            var path = new Gtk.TreePath.from_string(path_string);
//            if (path == null)
//                return;

//            Gtk.TreeIter iter;
//            if (!_controlsModel.get_iter(out iter, path))
//                return;

//            string? key = null;
//            _controlsModel.get(iter, 0, out key);
//            if (key == null)
//                return;

//            _controlsModel.set(iter, 2, 0);
//            _settings.set_int(key, 0);
//        }

//        private void ThemeComboChangedCb(Gtk.ComboBox widget)
//        {
//            Gtk.TreeIter iter;
//            widget.get_active_iter(out iter);
//            string theme;
//            widget.model.get(iter, 1, out theme);
//            _view.theme = theme;
//            _preview.theme = theme;
//            if (_themePreview != null)
//                _themePreview.theme = theme;
//            _settings.set_string("theme", theme);
//        }

//        private void FillHeightSpinnerValueChangedCb(Gtk.SpinButton spin)
//        {
//            int value = spin.get_value_as_int();
//            _settings.set_int("line-fill-height", value);
//        }

//        private void FillProbSpinnerValueChangedCb(Gtk.SpinButton spin)
//        {
//            int value = spin.get_value_as_int();
//            _settings.set_int("line-fill-probability", value);
//        }

//        private void StartingLevelValueChangedCb(Gtk.SpinButton spin)
//        {
//            int value = spin.get_value_as_int();
//            _settings.set_int("starting-level", value);
//        }

//        private void PauseCb()
//        {
//            if (_game != null)
//                _game.paused = !_game.paused;
//        }

//        private void QuitCb()
//        {
//            _window.destroy();
//        }

//        private void ManetteDeviceConnectedCb(Manette.Device manette_device)
//        {
//            manette_device.button_press_event.connect(ManetteButtonPressEventCb);
//            manette_device.button_release_event.connect(ManetteButtonReleaseEventCb);
//        }

//        private void ManetteButtonPressEventCb(Manette.Event event_b)
//        {
//            if (_game == null)
//                return;

//            uint16 button;
//            if (!event_b.get_button(out button))
//                return;

//            if (button == InputEventCode.BTN_START || button == InputEventCode.BTN_SELECT)
//            {
//                if (!_game.game_over)
//                    _game.paused = !_game.paused;
//                return;
//            }

//            if (_game.paused)
//                return;

//            if (button == InputEventCode.BTN_DPAD_LEFT)
//            {
//                _game.move_left();
//                return;
//            }
//            else if (button == InputEventCode.BTN_DPAD_RIGHT)
//            {
//                _game.move_right();
//                return;
//            }
//            else if (button == InputEventCode.BTN_A)
//            {
//                _game.rotate_left();
//                return;
//            }
//            else if (button == InputEventCode.BTN_B)
//            {
//                _game.rotate_right();
//                return;
//            }
//            else if (button == InputEventCode.BTN_DPAD_DOWN)
//            {
//                _game.set_fast_forward(true);
//                return;
//            }
//            else if (button == InputEventCode.BTN_DPAD_UP)
//            {
//                _game.drop();
//                return;
//            }
//        }

//        private void ManetteButtonReleaseEventCb(Manette.Event event_b)
//        {
//            if (_game == null)
//                return;

//            uint16 button;
//            if (!event_b.get_button(out button))
//                return;

//            if (button == InputEventCode.BTN_DPAD_LEFT ||
//                button == InputEventCode.BTN_DPAD_RIGHT)
//            {
//                _game.stop_moving();
//                return;
//            }
//            else if (button == InputEventCode.BTN_DPAD_DOWN)
//            {
//                _game.set_fast_forward(false);
//                return;
//            }
//        }

//        private bool KeyPressEventCb(Gtk.Widget widget, Gdk.EventKey event_b)
//        {
//            var keyval = UpperKey(event_b.keyval);

//            if (_game == null)
//            {
//                // Pressing pause with no game will start a new game.
//                if (keyval == UpperKey(_settings.get_int("key-pause")))
//                {
//                    NewGame();
//                    return true;
//                }

//                return false;
//            }

//            if (keyval == UpperKey(_settings.get_int("key-pause")))
//            {
//                if (!_game.game_over)
//                    _game.paused = !_game.paused;
//                return true;
//            }

//            if (_game.paused)
//                return false;

//            if (keyval == UpperKey(_settings.get_int("key-left")))
//            {
//                _game.move_left();
//                return true;
//            }
//            else if (keyval == UpperKey(_settings.get_int("key-right")))
//            {
//                _game.move_right();
//                return true;
//            }
//            else if (keyval == UpperKey(_settings.get_int("key-rotate")))
//            {
//                if (_settings.get_boolean("rotate-counter-clock-wise"))
//                    _game.rotate_left();
//                else
//                    _game.rotate_right();
//                return true;
//            }
//            else if (keyval == UpperKey(_settings.get_int("key-down")))
//            {
//                _game.set_fast_forward(true);
//                return true;
//            }
//            else if (keyval == UpperKey(_settings.get_int("key-drop")))
//            {
//                _game.drop();
//                return true;
//            }

//            return false;
//        }

//        private bool KeyReleaseEventCb(Gtk.Widget widget, Gdk.EventKey event_b)
//        {
//            var keyval = UpperKey(event_b.keyval);

//            if (_game == null)
//                return false;

//            if (keyval == UpperKey(_settings.get_int("key-left")) ||
//                keyval == UpperKey(_settings.get_int("key-right")))
//            {
//                _game.stop_moving();
//                return true;
//            }
//            else if (keyval == UpperKey(_settings.get_int("key-down")))
//            {
//                _game.set_fast_forward(false);
//                return true;
//            }

//            return false;
//        }

//        private uint UpperKey(uint keyval)
//        {
//            if (keyval > 255)
//                return keyval;
//            return ((char)keyval).toupper();
//        }

//        private void NewGameCb()
//        {
//            NewGame();
//        }

//        private void NewGame()
//        {
//            if (_game != null)
//            {
//                _game.stop();
//                SignalHandler.disconnect_matched(_game, SignalMatchType.DATA, 0, 0, null, null, this);
//            }

//            _game = new Game(20, 14, _settings.get_int("starting-level"), _settings.get_int("line-fill-height"), _settings.get_int("line-fill-probability"), _settings.get_boolean("pick-difficult-blocks"));
//            _game.pause_changed.connect(PauseChangedCb);
//            _game.shape_landed.connect(ShapeLandedCb);
//            _game.complete.connect(CompleteCb);
//            _preview.game = _game;
//            _view.game = _game;

//            _game.start();

//            UpdateScore();
//            _pauseAction.set_enabled(true);
//            _pausePlayButton.action_name = "app.pause";
//        }

//        private void PauseChangedCb()
//        {
//            if (_game.paused)
//            {
//                _pausePlayButtonImage.set_from_icon_name("media-playback-start-symbolic", Gtk.IconSize.DIALOG);
//                _pausePlayButton.tooltip_text = _("Unpause the game");
//            }
//            else
//            {
//                _pausePlayButtonImage.set_from_icon_name("media-playback-pause-symbolic", Gtk.IconSize.DIALOG);
//                _pausePlayButton.tooltip_text = _("Pause the game");
//            }
//        }

//        private void ShapeLandedCb(int[] lines, List<Block> line_blocks)
//        {
//            UpdateScore();
//        }

//        private void CompleteCb()
//        {
//            _pauseAction.set_enabled(false);
//            _pausePlayButtonImage.set_from_icon_name("view-refresh-symbolic", Gtk.IconSize.DIALOG);
//            _pausePlayButton.action_name = "app.new-game";
//            _pausePlayButton.tooltip_text = _("Start a new game");

//            if (_game.score > 0)
//            {
//                var date = new DateTime.now_local();
//                var entry = new HistoryEntry(date, _game.score);
//                _history.add(entry);
//                _history.save();

//                if (ShowScores(entry, true) == Gtk.ResponseType.OK)
//                    NewGame();
//            }
//        }

//        private int ShowScores(HistoryEntry? selected_entry = null, bool show_close = false)
//        {
//            var dialog = new ScoreDialog(_history, selected_entry, show_close);
//            dialog.modal = true;
//            dialog.transient_for = _window;

//            var result = dialog.run();
//            dialog.destroy();

//            return result;
//        }

//        private void UpdateScore()
//        {
//            var score = 0;
//            var level = 0;
//            var n_lines_destroyed = 0;

//            if (_game != null)
//            {
//                score = _game.score;
//                level = _game.level;
//                n_lines_destroyed = _game.n_lines_destroyed;
//            }

//            _scoreLabel.set_markup("<big>%d</big>".printf(score));
//            _levelLabel.set_markup("<big>%d</big>".printf(level));
//            _nDestroyedLabel.set_markup("<big>%d</big>".printf(n_lines_destroyed));
//        }

//        private void HelpCb()
//        {
//            try
//            {
//                Gtk.show_uri(_window.get_screen(), "help:quadrapassel", Gtk.get_current_event_time());
//            }
//            catch (Error e)
//            {
//                warning("Failed to show help: %s", e.message);
//            }
//        }

//        private void AboutCb()
//        {
//            string[] authors = { "GNOME Games Team", null };
//            string[] documenters = { "Angela Boyle", null };

//            Gtk.show_about_dialog(_window,
//                                   "program-name", _("Quadrapassel"),
//                                   "version", VERSION,
//                                   "comments", _("A classic game of fitting falling blocks together"),
//                                   "copyright", "Copyright © 1999 J. Marcin Gorycki, 2000–2015 Others",
//                                   "license-type", Gtk.License.GPL_2_0,
//                                   "authors", authors,
//                                   "documenters", documenters,
//                                   "translator-credits", _("translator-credits"),
//                                   "logo-icon-name", "org.gnome.Quadrapassel",
//                                   "website", "https://wiki.gnome.org/Apps/Quadrapassel",
//                                   null);
//        }

//        private void MenuCb()
//        {
//            _menuButton.activate();
//        }

//        private void ScoresCb()
//        {
//            ShowScores();
//        }

//        public static int Main(string[] args)
//        {
//            Intl.setlocale(LocaleCategory.ALL, "");
//            Intl.bindtextdomain(GETTEXT_PACKAGE, LOCALEDIR);
//            Intl.bind_textdomain_codeset(GETTEXT_PACKAGE, "UTF-8");
//            Intl.textdomain(GETTEXT_PACKAGE);

//            var context = new OptionContext("");

//            context.add_group(Gtk.get_option_group(true));
//            context.add_group(Clutter.get_option_group_without_init());

//            try
//            {
//                context.parse(ref args);
//            }
//            catch (Error e)
//            {
//                stderr.printf("%s\n", e.message);
//                return Posix.EXIT_FAILURE;
//            }

//            Environment.set_application_name(_("Quadrapassel"));

//            Gtk.Window.set_default_icon_name("quadrapassel");

//            try
//            {
//                GtkClutter.init_with_args(ref args, "", new OptionEntry[0], null);
//            }
//            catch (Error e)
//            {
//                var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.MODAL, Gtk.MessageType.ERROR, Gtk.ButtonsType.NONE, "Unable to initialize Clutter:\n%s", e.message);
//                dialog.set_title(Environment.get_application_name());
//                dialog.run();
//                dialog.destroy();
//                return Posix.EXIT_FAILURE;
//            }

//            var app = new Quadrapassel();
//            return app.run(args);
//        }
//    }
//}
