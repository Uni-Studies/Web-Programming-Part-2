using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Services;

public class ServiceResultExtension<T> 
where T : class, new()
{
    public static ServiceResult<T> Failure(T data, ModelStateDictionary errors)
    {
        var errorsList = new List<Error>();
        foreach (var error in errors)
        {
            if(error.Value.Errors.Count > 0)
            {
                errorsList.Add(new Error
                {
                    Key = error.Key,
                    Messages = error.Value.Errors.Select(e => e.ErrorMessage).ToList()
                });
                
            }
        }
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Data = data,
            Errors = errorsList
        };
    }
}
