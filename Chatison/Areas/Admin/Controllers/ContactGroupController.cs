using Chatison.Helpers;
using Chatison.Infrastructure.Managers;
using Chatison.Utilities;
using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Group;
using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Chatison.Areas.Admin.Controllers
{
    public class ContactGroupController : BaseController
    {
        private readonly IGroupManager _groupManager;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ContactGroupController(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Manage(JqDataTableRequestVm model)
        {
            var pagedResult = await _groupManager.GetAsync(model);

            return new JsonCamelCaseResult
            {
                Data = pagedResult
            };
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddGroupVm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(Utility.GetErrorResponse(ModelState.GetErrorList()), JsonRequestBehavior.AllowGet);
            }

            if (await _groupManager.IsExistsAsync(model.Name))
            {
                return Json(Utility.GetErrorResponse("Another group with same name already exists."), JsonRequestBehavior.AllowGet);
            }

            try
            {
                await _groupManager.AddAsync(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(Utility.GetErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
            }

            return Json(Utility.GetSuccessResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditGroupVm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(Utility.GetErrorResponse(ModelState.GetErrorList()), JsonRequestBehavior.AllowGet);
            }

            if (await _groupManager.IsExistsAsync(model.Id, model.Name))
            {
                return Json(Utility.GetErrorResponse("Another group with same name already exists."), JsonRequestBehavior.AllowGet);
            }

            try
            {
                await _groupManager.EditAsync(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(Utility.GetErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
            }

            return Json(Utility.GetSuccessResponse(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _groupManager.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(Utility.GetErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
            }

            return Json(Utility.GetSuccessResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}