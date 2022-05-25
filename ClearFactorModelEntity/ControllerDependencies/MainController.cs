using ClearFactorModelEntity.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearFactorModelEntity.ControllerDependencies
{
    [ApiController]
    public class MainController<TModel, TRepository> : ControllerBase where TModel : class
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected IRepository<TModel> Repository { get; set; }
        protected ILogger Logger { get; set; }

        public MainController(IUnitOfWork unitOfWork, ILogger logger)
        {
            UnitOfWork = unitOfWork;
            Repository = unitOfWork.GetRepository<TModel, TRepository>();

            Logger = logger;
        }

        protected async Task<ActionResult<IEnumerable<TModel>>> BaseGet()
        {
            try
            {
                return Ok(await Repository.Get().ToListAsync());
            }
            catch (Exception)
            {
                return base.BadRequest("Bad Request");
            }
        }

        protected async Task<ActionResult<TModel>> BaseGetById(Guid id)
        {
            try
            {
                return base.Ok(await Repository.GetById(id));
            }
            catch (Exception ex) when (ex.Message.Contains("Sequence contains no elements"))
            {
                return base.BadRequest($"{typeof(TModel).Name} Id does not exist.");
            }
            catch (Exception)
            {
                return base.BadRequest("Bad Request");
            }
        }

        protected async Task<ActionResult> BasePost(TModel incoming)
        {
            string objectType = typeof(TModel).ToString().Split('.').Last();

            try
            {
                await Repository.Create(incoming);
                await Repository.Save();

                return base.Ok(objectType + " Created Successfully.");
            }
            catch (Exception ex)
            {
                return base.BadRequest("Bad Request");
            }
        }

        protected async Task<ActionResult> BasePut(TModel incoming)
        {
            string objectType = typeof(TModel).ToString().Split('.').Last();

            try
            {
                await Repository.Edit(incoming);
                await Repository.Save();

                return base.Ok(objectType + " Edited Successfully");
            }
            catch (Exception)
            {
                return base.BadRequest("Bad Request");
            }
        }

        protected async Task<ActionResult> BaseDelete(Guid id)
        {
            string objectType = typeof(TModel).ToString().Split('.').Last();

            try
            {
                await Repository.Delete(id);
                await Repository.Save();

                return base.Ok(objectType + " Deleted Successfully.");
            }
            catch (Exception)
            {
                return base.BadRequest("Bad Request");
            }
        }
    }
}
