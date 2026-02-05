using System;
using System.Text.Json.Serialization;


namespace Common.Entities;

public class SocialNetwork: BaseEntity
{
    public int UserId { get; set; }
    public string Type { get; set; }
    public string Account { get; set; }
    public string Link { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
}
