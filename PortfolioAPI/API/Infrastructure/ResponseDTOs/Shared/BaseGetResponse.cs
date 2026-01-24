using System;
using System.Collections.Generic;

namespace API.Infrastructure.ResponseDTOs.Shared;

public class BaseGetResponse<E>
{
    public List<E> Items { get; set; }
    public PagerResponse Pager { get; set; }
    public string OrderBy { get; set; }
    public bool SortAscending { get; set; }
}
