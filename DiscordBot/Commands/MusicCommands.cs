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
    private Dictionary<ulong, List<LavalinkTrack>> _queue = new Dictionary<ulong, List<LavalinkTrack>>();

    [Command]
    [Description("Pauses the current track.")]
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
    [Description("Plays a song by search request or URL.")]
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
        _queue.TryAdd(lava.guildConnection.Guild.Id, new List<LavalinkTrack>());
        _queue[lava.guildConnection.Guild.Id].Add(track);
        await context.RespondAsync($"Add {track.Title} to queue.");
      }
      else
      {
        await lava.guildConnection.PlayAsync(track);
        await context.RespondAsync($"Now playing {track.Title}!");
      }
      lava.guildConnection.PlaybackFinished -= PlayNext;
      lava.guildConnection.PlaybackFinished += PlayNext;
    }

    [Command]
    [Description("Resumes the current track.")]
    public async Task Resume(CommandContext context) 
    {
      var lava = LavaUtility.GetLavaConnections(context);
      await lava.guildConnection.ResumeAsync();
    }

    [Command]
    [Description("Returns the current queue of songs.")]
    public async Task Queue(CommandContext context)
    {

      if (_queue.TryGetValue(context.Guild.Id, out var queue) && queue.Count() > 0)
      {
        await context.RespondAsync(string.Join('\n', queue.Select(s => s.Title).ToArray()));
      }
      else
      {
        await context.RespondAsync("The queue is empty");
      }
    }

    [Command]
    [Description("Skips the current track.")]
    public async Task Skip(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      await lava.guildConnection.StopAsync();
    }

    [Command]
    [Description("Stops Vinyl and clears the queue.")]
    public async Task Stop(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      _queue.Clear();
      await lava.guildConnection.StopAsync();
    }

    [Command]
    [Description("Returns the current playback position and track length.")]
    public async Task Time(CommandContext context)
    {
      var lava = LavaUtility.GetLavaConnections(context);
      var length = lava.guildConnection.CurrentState.CurrentTrack.Length;
      var position = lava.guildConnection.CurrentState.PlaybackPosition;
      await context.RespondAsync($"{position.ToString("hh\\:mm\\:ss")}/{length.ToString("hh\\:mm\\:ss")}");
    }

    [Command]
    [Description("Sets Vinyl's volume from a given value between 5 and 100.")]
    public async Task Volume(CommandContext context, int value)
    {   
      if (value < 5)
      {
        await context.RespondAsync("Volume must be greater than or equal to 5.");
        return;
      }
      else if(value > 100)
      {
        await context.RespondAsync("Volume must be less than or equal to 100.");
        return;
      }

      var lava = LavaUtility.GetLavaConnections(context);
      await lava.guildConnection.SetVolumeAsync(value);
      await context.RespondAsync($"Volume set to {value}");
    }

    private async Task PlayNext(LavalinkGuildConnection guild, TrackFinishEventArgs e)
    {
      if (_queue.TryGetValue(guild.Guild.Id, out var queue) && queue.Count() > 0)
      {
        var track = queue.First();
        await guild.PlayAsync(track);
        queue.RemoveAt(0);
      }
      else
      {
        await guild.DisconnectAsync();
      }
    }
  }
}
