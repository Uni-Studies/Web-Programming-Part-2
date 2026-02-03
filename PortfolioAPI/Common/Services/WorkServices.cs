using System;
using Common.Entities;

namespace Common.Services;

public class WorkServices : BaseServices<Work>
{
    public Work GetWorkByUserId(int userId, int workId)
    {
        var work = GetById(workId);
        if(work is null)
            throw new ArgumentException("Work not found!");

        if(work.UserId != userId)
            throw new ArgumentException("Work has another user!"); 
               
        return work;
    }
}
