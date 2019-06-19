using Chatison.DataLayer;
using Chatison.Helpers;
using Chatison.Infrastructure.Managers;
using Chatison.Utilities;
using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Contact;
using ExcelDataReader;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Chatison.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private Logger Logger => LogManager.GetCurrentClassLogger();
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IGroupManager _groupManager;
        private readonly IContactManager _contactManager;
        private readonly ISelectListManager _selectListManager;

        public ContactController(IGroupManager groupManager,
            IContactManager contactManager,
            ISelectListManager selectListManager)
        {
            _groupManager = groupManager;
            _contactManager = contactManager;
            _selectListManager = selectListManager;
        }

        [HttpGet]
        public async Task<ActionResult> Manage(int id)
        {
            ViewBag.GroupName = await _groupManager.GetNameAsync(id);
            ViewBag.GroupId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Manage(JqDataTableRequestVm model, int? groupId)
        {
            var pagedResult = await _contactManager.GetAsync(model, groupId);

            return new JsonCamelCaseResult
            {
                Data = pagedResult
            };
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddContactVm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(Utility.GetErrorResponse(ModelState.GetErrorList()));
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone.RemovePhoneMask(),
                Status = Constants.RecordStatus.Active
            };

            model.Source = Constants.ContactSource.Manually;

            var result = await UserManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return Json(Utility.GetErrorResponse(result.Errors));
            }

            try
            {
                await UserManager.AddToRoleAsync(user.Id,
                      model.IsAdmin ? Constants.UserRoles.Admin : Constants.UserRoles.User);

                await _contactManager.AddAsync(model, user.Id);

                return Json(Utility.GetSuccessResponse());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                await UserManager.DeleteAsync(user);
                return Json(Utility.GetErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            return new JsonCamelCaseResult
            {
                Data = await _contactManager.GetForEditAsync(id)
            };
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditContactVm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(Utility.GetErrorResponse(ModelState.GetErrorList()), JsonRequestBehavior.AllowGet);
            }

            if (await UserManager.Users.AnyAsync(x => x.Email.Equals(model.Email) && !x.Id.Equals(model.Id)))
            {
                return Json(Utility.GetErrorResponse("Another contact with same email already exists."), JsonRequestBehavior.AllowGet);
            }

            var user = await UserManager.FindByIdAsync(model.Id);

            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.Phone.RemovePhoneMask();

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Json(Utility.GetErrorResponse(result.Errors));
            }

            try
            {
                await _contactManager.EditAsync(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(Utility.GetErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
            }

            return Json(Utility.GetSuccessResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            var deletedToken = "_del_" + Guid.NewGuid().ToString("N");

            user.UserName = user.UserName + deletedToken;
            user.Email = user.Email + deletedToken;
            user.PhoneNumber = user.PhoneNumber + deletedToken;
            user.Status = Constants.RecordStatus.Deleted;

            var result = await UserManager.UpdateAsync(user);

            return Json(!result.Succeeded
                ? Utility.GetErrorResponse(result.Errors)
                : Utility.GetSuccessResponse());
        }

        [HttpGet]
        public ActionResult Import(int? groupId = null)
        {
            ViewBag.GroupId = groupId;

            return View();
        }

        [HttpPost]
        public ActionResult MapFields(HttpPostedFileBase uploadedFile, int? groupId)
        {
            if (uploadedFile == null
                || !uploadedFile.FileName.EndsWith(".xls")
                    && !uploadedFile.FileName.EndsWith(".xlsx")
                    && !uploadedFile.FileName.EndsWith(".csv"))
            {
                this.SetErrorResponse("Please upload a xls or csv file", true);
                return RedirectToAction("Import");
            }

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(uploadedFile.FileName);
            var dirPath = HostingEnvironment.ApplicationPhysicalPath + "\\Content\\Uploads\\Temp\\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            uploadedFile.SaveAs(dirPath + fileName);

            var columns = new List<SelectListItemVm>();

            using (var stream = uploadedFile.InputStream)
            {
                using (var reader = fileName.EndsWith(".csv")
                    ? ExcelReaderFactory.CreateCsvReader(stream)
                    : ExcelReaderFactory.CreateReader(stream))
                {
                    if (reader.Read())
                    {
                        var totalFieldCount = reader.FieldCount;
                        for (var i = 0; i < totalFieldCount; i++)
                        {
                            columns.Add(new SelectListItemVm
                            {
                                Key = reader.GetString(i),
                                Name = reader.GetString(i)
                            });
                        }
                    }
                }
            }

            ViewBag.FileName = fileName;

            ViewBag.Columns = columns.OrderBy(x => x.Name).ToList();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Import(ImportContactVm model)
        {
            await Task.Delay(1);

            var providers = _selectListManager.GetMobileProviders().ToList();

            var contacts = new List<AddContactVm>();

            var filePath = HostingEnvironment.ApplicationPhysicalPath + "\\Content\\Uploads\\Temp\\" +
                           model.UploadedFileName;


            var columns = new List<string>();
            var firstNameIndex = -1;
            var lastNameIndex = -1;
            var emailIndex = -1;
            var phoneIndex = -1;
            var providerIndex = -1;

            var isFirstRow = true;
            var counter = 0;

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = model.UploadedFileName.EndsWith(".csv")
                    ? ExcelReaderFactory.CreateCsvReader(stream)
                    : ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        counter++;

                        if (isFirstRow)
                        {
                            isFirstRow = false;

                            var totalFieldCount = reader.FieldCount;
                            for (var i = 0; i < totalFieldCount; i++)
                            {
                                columns.Add(reader.GetString(i));
                            }

                            firstNameIndex = columns.IndexOf(model.FirstNameColumn);
                            lastNameIndex = columns.IndexOf(model.LastNameColumn);
                            emailIndex = columns.IndexOf(model.EmailColumn);
                            phoneIndex = columns.IndexOf(model.PhoneColumn);
                            providerIndex = columns.IndexOf(model.ProviderColumn);

                            continue;
                        }

                        if (firstNameIndex == -1 || lastNameIndex == -1 || emailIndex == -1 || phoneIndex == -1)
                        {
                            this.SetErrorResponse("Required columns not found. Please try again with valid file.", true);
                            return RedirectToAction("Import");
                        }

                        try
                        {
                            int? providerId = null;
                            if (providerIndex != -1)
                            {
                                providerId = providers.FirstOrDefault(x => x.Name.Equals(reader.GetString(providerIndex).ToString()))?.Id;
                            }

                            var contact = new AddContactVm
                            {
                                FirstName = reader[firstNameIndex].ToString(),
                                LastName = reader[lastNameIndex].ToString(),
                                Email = reader[emailIndex].ToString(),
                                Phone = reader[phoneIndex].ToString(),
                                ProviderId = providerId,
                                GroupIds = new List<int> { model.GroupId }
                            };

                            if (string.IsNullOrEmpty(contact.FirstName)
                                || string.IsNullOrEmpty(contact.LastName)
                                || string.IsNullOrEmpty(contact.Email)
                                || string.IsNullOrEmpty(contact.Phone))
                            {
                                Logger.Error($"Row {counter} is empty");
                                continue;
                            }

                            contacts.Add(contact);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error on row {counter} parsing");
                            Logger.Error(ex);
                        }
                    }
                }
            }

            var successfullyImported = 0;

            foreach (var contact in contacts)
            {
                try
                {

                    var user = new ApplicationUser
                    {
                        UserName = contact.Email,
                        Email = contact.Email,
                        PhoneNumber = contact.Phone.RemovePhoneMask(),
                        Status = Constants.RecordStatus.Active
                    };

                    var result = await UserManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        Logger.Error($"Failed to import: {JsonConvert.SerializeObject(contact)}");
                        Logger.Error(result.Errors);
                    }

                    await UserManager.AddToRoleAsync(user.Id, Constants.UserRoles.User);

                    contact.Source = Constants.ContactSource.Imported;

                    await _contactManager.AddAsync(contact, user.Id);

                    successfullyImported++;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to import: {JsonConvert.SerializeObject(contact)}");
                    Logger.Error(ex);
                }
            }

            if (successfullyImported > 0)
            {
                this.SetSuccessResponse($"Total {successfullyImported} records imported successfully. Please check the log for failed records detail.", true);
            }
            else
            {
                this.SetErrorResponse("No record get imported. Please the check log for the error details.", true);
            }

            return RedirectToAction("Import");
        }

        [HttpGet]
        public async Task<ActionResult> Export(int? groupId)
        {
            var contacts = (await _contactManager.GetAllAsync(groupId)).ToList();

            var dataTable = new DataTable($"Chatison_{DateTime.Now.Ticks}");
            dataTable.Columns.Add("First Name");
            dataTable.Columns.Add("Last Name");
            dataTable.Columns.Add("Email Name");
            dataTable.Columns.Add("Phone Name");
            dataTable.Columns.Add("Created");
            dataTable.Columns.Add("Group");

            foreach (var contact in contacts)
            {
                var dr = dataTable.NewRow();

                dr[0] = contact.FirstName;
                dr[1] = contact.LastName;
                dr[2] = contact.Email;
                dr[3] = contact.Phone;
                dr[4] = contact.CreatedAt.ToShortDateString();
                if (contact.GroupNames != null)
                {
                    dr[5] = string.Join(",", contact.GroupNames);
                }

                dataTable.Rows.Add(dr);
            }

            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                //worksheet.Cells["A1"].LoadFromCollection(Collection: contacts, PrintHeaders: true);
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment;  filename=Chatison_{DateTime.Now.Ticks}.xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }

            return RedirectToAction("manage", "contactGroup", new { area = "admin" });
        }
    }
}