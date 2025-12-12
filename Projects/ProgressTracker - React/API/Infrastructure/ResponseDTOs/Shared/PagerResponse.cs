using System;
using API.Infrastructure.RequestDTOs.Shared;
using Microsoft.Identity.Client;

namespace API.Infrastructure.ResponseDTOs.Shared;

public class PagerResponse : PagerRequest
{
    public int Count { get; set; }
}
