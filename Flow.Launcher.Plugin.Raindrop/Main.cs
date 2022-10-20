using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Flow.Launcher.Plugin;
using Flow.Launcher.Plugin.Raindrop.Core;
using Flow.Launcher.Plugin.Raindrop.Controls;

namespace Flow.Launcher.Plugin.Raindrop
{
    public class Raindrop : IPlugin, ISettingProvider, IResultUpdated
    {
        private PluginInitContext _context;
        private Settings _settings;
        private RaindropApi _api;
        private CancellationTokenSource _cancellationTokenSource;

        public event ResultUpdatedEventHandler ResultsUpdated;

        public Control CreateSettingPanel()
        {
            return new SettingsView(_settings);
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings>();
            _api = new RaindropApi(_settings.Token);
        }

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrEmpty(_settings!.Token))
            {
                return new List<Result>
                {
                    new Result
                    {
                        Title = "Raindrop",
                        SubTitle = "Failed: API Token is empty.",
                        IcoPath = "Images/app.png"
                    }
                };
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Run(async delegate
            {
                var raindrops = await _api.SearchForAsync(query.Search, token);
                var results = new List<Result>();

                if (raindrops.Count <= 0)
                {
                    results.Add(new Result
                    {
                        Title = "Raindrop",
                        SubTitle = "Nothing found",
                        IcoPath = "Images/app.png"
                    });
                }
                else
                {
                    foreach (var raindrop in raindrops)
                    {
                        token.ThrowIfCancellationRequested();
                        results.Add(CreateResult(query.Search, raindrop));
                    }
                }

                ResultsUpdated?.Invoke(this, new ResultUpdatedEventArgs
                {
                    Results = results,
                    Query = query,
                    Token = token
                });

            }, token);

            return new List<Result> { };
        }

        private Result CreateResult(string keyword, Core.Raindrop raindrop)
        {
            return new Result
            {
                Title = raindrop.Title,
                TitleHighlightData = _context?.API.FuzzySearch(keyword, raindrop.Title).MatchData,
                SubTitle = raindrop.Link,
                IcoPath = "Images/app.png",
                Action = delegate
                    {
                        _context.API.OpenUrl(raindrop.Link);
                        return true;
                    }
            };
        }
    }
}