using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Configs
{
  public struct Config
  {
    [JsonProperty("token")]
    public string Token { get; private set; }

    [JsonProperty("dev-token")]
    public string DevToken { get; private set; }

    [JsonProperty("prefix")]
    public string Prefix { get; private set; }

    [JsonProperty("dev-prefix")]
    public string DevPrefix { get; private set; }
  }
}
