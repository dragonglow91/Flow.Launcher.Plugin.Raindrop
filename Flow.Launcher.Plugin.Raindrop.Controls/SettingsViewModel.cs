using Flow.Launcher.Plugin.Raindrop.Core;

namespace Flow.Launcher.Plugin.Raindrop.Controls
{
    public class SettingsViewModel
    {
        private Settings _settings;

        public SettingsViewModel(Settings settings)
        {
            _settings = settings;
        }

        public string Token
        {
            get => _settings.Token;
            set => _settings.Token = value;
        }
    }
}
