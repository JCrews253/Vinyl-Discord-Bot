using DiscordBot.Attributes;
using DiscordBot.Utilities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
  [VoiceConnection]
  [LavaConnection]
  public class MusicCommands : BaseCommandModule
  {
    private List<LavalinkTrack> _queue = new List<LavalinkTrack>();

    [Command]
    public async Task Pause(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);

      if (lava.guildConnection.CurrentState.CurrentTrack == null)
      {
        await context.RespondAsync("There are no tracks loaded.");
        return;
      }

      await lava.guildConnection.PauseAsync();
    }

    [Command]
    public async Task Play(CommandContext context, [RemainingText] string request = "")
    {
      var lava = LavaUtility.GetLavaConnections(context);

      LavalinkLoadResult loadResult;
      if (Uri.IsWellFormedUriString(request, UriKind.Absolute))
      {
        loadResult = await lava.node.Rest.GetTracksAsync(new Uri(request));
      }
      else
      {
        loadResult = await lava.node.Rest.GetTracksAsync(request);
      }

      if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
          || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
      {
        await context.RespondAsync($"Track search failed for {request}.");
        return;
      }

      var track = loadResult.Tracks.First();

      if (lava.guildConnection.CurrentState.CurrentTrack != null)
      {
        _queue.Add(track);
        await context.RespondAsync($"Add {track.Title} to queue.");
        lava.guildConnection.PlaybackFinished -= PlayNext;
        lava.guildConnection.PlaybackFinished += PlayNext;
      }
      else
      {
        await lava.guildConnection.PlayAsync(track);
        await context.RespondAsync($"Now playing {track.Title}!");
      }
    }

    private async Task PlayNext(LavalinkGuildConnection guild, TrackFinishEventArgs e)
    {
      if (_queue.Count() > 0)
      {
        var track = _queue.First();
        await guild.PlayAsync(track);
        _queue.RemoveAt(0);
      }
    }

    [Command]
    public async Task Queue(CommandContext context)
    {
      if (_queue.Count() > 0)
      {
        await context.RespondAsync(string.Join('\n', _queue.Select(s => s.Title).ToArray()));
      }
      else
      {
        await context.RespondAsync("The queue is empty");
      }
    }

    [Command]
    public async Task Skip(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      await lava.guildConnection.StopAsync();
    }

    [Command]
    public async Task Stop(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      _queue.Clear();
      await lava.guildConnection.StopAsync();
    }

    [Command]
    public async Task Time(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      var length = lava.guildConnection.CurrentState.CurrentTrack.Length;
      var position = lava.guildConnection.CurrentState.PlaybackPosition;
      await context.RespondAsync($"{position.ToString("hh\\:mm\\:ss")}/{length.ToString("hh\\:mm\\:ss")}");
    }
  }
}
