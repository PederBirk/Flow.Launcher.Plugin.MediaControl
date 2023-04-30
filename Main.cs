using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.MediaControl
{
    public class MediaControl : IPlugin
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            if(string.IsNullOrWhiteSpace(query.Search))
            {
                return Actions().Select(x => x.AsResult()).ToList();
            }
            var filteredResults = Actions().Where(x => x.Keywords.Any(y => y.Contains(query.Search.ToLower())));
            if(filteredResults.Any())
            {
                return filteredResults.Select(x => x.AsResult()).ToList();
            }
            return new List<Result>();
        }

        public static IEnumerable<MediaControlAction> Actions()
        {
            yield return new ()
            {
                Action = MediaPlaybackAction.Toggle,
                Keywords = new List<string>{"play", "pause", "toggle"},
                Title = "Toggle playback"
            };
             yield return new ()
            {
                Action = MediaPlaybackAction.Next,
                Keywords = new List<string>{"next", "skip"},
                Title = "Next track"
            };
             yield return new ()
            {
                Action = MediaPlaybackAction.Previous,
                Keywords = new List<string>{"back", "previous"},
                Title = "Previous track"
            };
             yield return new ()
            {
                Action = MediaPlaybackAction.Stop,
                Keywords = new List<string>{"stop"},
                Title = "Stop playback"
            };
}
    }

    public class MediaControlAction
    {
        public required IEnumerable<string> Keywords { get; init; }
        public required MediaPlaybackAction Action { get; init; }
        public required string Title { get; init; }
        public Result AsResult()
        {
            return new Result
            {
                Action = (context) => { MediaController.Execute(Action); return true; },
                Title = Title,
            };
        }
    }
}