using System;

namespace API.Infrastructure.RequestDTOs.SocialNetwork;

public class SocialNetworkRequest
{
    public string Type { get; set; }
    public string Account { get; set; }
    public string Link { get; set; }
}
