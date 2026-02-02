using System;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Work;
using API.Infrastructure.ResponseDTOs.Work;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : BaseCRUDController<Work, WorkServices, WorkRequest, WorkGetRequest, WorkGetResponse>
    {
        protected override void PopulateEntity(Work item, WorkRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.Sphere = model.Sphere;
            item.Occupation = model.Occupation;
            item.Location = model.Location;
            item.Salary = model.Salary;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
        }

        protected override Expression<Func<Work, bool>> GetFilter(WorkGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new WorkGetFilterRequest();    
            return 
                p => 
                    (string.IsNullOrEmpty(model.Filter.Sphere) || p.Sphere.Contains(model.Filter.Sphere)) &&
                    (string.IsNullOrEmpty(model.Filter.Occupation) || p.Occupation.Contains(model.Filter.Occupation)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || p.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.Company) || p.Company.Contains(model.Filter.Company)) &&
                
                    (!model.Filter.SalaryFrom.HasValue || p.Salary >= model.Filter.SalaryFrom.Value) &&
                    (!model.Filter.SalaryTo.HasValue || p.Salary <= model.Filter.SalaryTo.Value);
        
                    /* (!model.Filter.CompletionStatus.HasValue ||
                    (p.CompletionStatus == model.Filter.CompletionStatus); */
        } 

        protected override void PopulateGetResponse(WorkGetRequest request, WorkGetResponse response)
        {
            response.Filter = request.Filter;
        }
    }
}
