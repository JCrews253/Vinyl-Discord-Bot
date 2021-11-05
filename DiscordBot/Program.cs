﻿using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Lavalink;
using DiscordBot.Configs;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Diagnostics;

namespace DiscordBot
{
  public class Program
  {
    private static Config _config;
    private static DiscordClient _client;
    private static CommandsNextExtension _commands;

    static async Task Main(string[] args)
    {
      StartLavalink();
      // Read config file
      using(var fs = File.OpenRead(@"Configs\config.json"))
      using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
      {
        var json = await sr.ReadToEndAsync();
        _config = JsonConvert.DeserializeObject<Config>(json);
      }

      // Create discord client
      _client = new DiscordClient(new DiscordConfiguration()
      {
        Token = _config.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        Intents = DiscordIntents.AllUnprivileged    
      });

      var commmandConfig = new CommandsNextConfiguration
      {
        StringPrefixes = new[] { _config.Prefix }
      };
      _commands = _client.UseCommandsNext(commmandConfig);

      var lava = _client.UseLavalink();

      RegisterCommands();

      _client.Ready += SetStatus;
      await _client.ConnectAsync();
      await ConnectLavaNode(lava);
      await Task.Delay(-1);
    }

    static void RegisterCommands()
    {
      _commands.RegisterCommands<PingCommand>();
      _commands.RegisterCommands<MusicCommands>();
      _commands.RegisterCommands<ChatCommands>();
    }

    public static async Task ConnectLavaNode(LavalinkExtension lava)
    {
      var lavaEndpoint = new ConnectionEndpoint
      {
        Hostname = "127.0.0.1",
        Port = 2333
      };

      var lavalinkConfig = new DSharpPlus.Lavalink.LavalinkConfiguration
      {
        Password = "youshallnotpass",
        RestEndpoint = lavaEndpoint,
        SocketEndpoint = lavaEndpoint
      };
      await lava.ConnectAsync(lavalinkConfig);
    }

    static async Task SetStatus(DiscordClient client, ReadyEventArgs e)
    {
      await client.UpdateStatusAsync(activity: new DiscordActivity(".help"));
    }

    static void StartLavalink()
    {
      string path = Directory.GetCurrentDirectory();
      path += "Lavalink.jar";
      var lavaLink = new Process();
      lavaLink.StartInfo.UseShellExecute = true;
      lavaLink.StartInfo.FileName = "Lavalink.jar";
      lavaLink.StartInfo.Arguments = $"-jar {path}";
      lavaLink.Start();
    }
  }
}
