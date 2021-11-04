using DSharpPlus.Net;

namespace DiscordBot.Configs
{
  internal class LavalinkConfiguration
  {
    public string Password { get; set; }
    public ConnectionEndpoint RestEndpoint { get; set; }
    public ConnectionEndpoint SocketEndpoint { get; set; }
  }
}