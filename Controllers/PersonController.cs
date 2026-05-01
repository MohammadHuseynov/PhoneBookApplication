using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneBookApplication.ApplicationServices.DTOs;
using PhoneBookApplication.ApplicationServices.Services.Contracts;
using PhoneBookApplication.Models.DomainModels.PersonAggregates;
using System.Globalization;
using System.Linq;
using System.Net;
using PhoneBookApplication.Models;


namespace PhoneBookApplication.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonApplicationService _personApplicationService;

        public PersonController(IPersonApplicationService personApplicationService)
        {
            _personApplicationService = personApplicationService;
        }

        #region [- Index() -]
        // GET: Persons/Index
        public async Task<IActionResult> Index()
        {
            var response = await _personApplicationService.GetAllPerson();

            if (!response.IsSuccessful)
                ViewBag.ErrorMessage = response.ErrorMessage ?? "An error occurred.";

            return View(response.Result ?? new GetAllPersonDto());
        }
        #endregion



        #region [- SearchIndex() -]
        // GET: Persons/SearchIndex
        [HttpGet]
        public async Task<IActionResult> SearchIndex(string term)
        {


            var response = await _personApplicationService.SearchPerson(term);

            var viewModel = new GetAllPersonDto()
            {
                Term = term,
                GetByIdPersonDto = response.Result ?? new List<GetByIdPersonDto>()

            };

            return View(viewModel);


        }
        #endregion

        #region [- Details() -]
        // GET: /Person/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var response = await _personApplicationService.GetByIdPerson(new GetByIdPersonDto { Id = id });
            if (!response.IsSuccessful || response.Result == null)
                return NotFound();

            return View(response.Result);
        }
        #endregion


        #region [- Create(GET) -]
        // GET: /Person/Create
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region [- Create(POST) -]
        // POST: /Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostPersonDto postPersonDto)
        {
            if (!ModelState.IsValid)
                return View(postPersonDto);
            
            var response = await _personApplicationService.Post(postPersonDto);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Failed to create Contact.");
                return View(postPersonDto);
            }

            TempData["SuccessMessage"] = "Contact created successfully!";
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region [- Edit(GET) -]
        // GET: /Person/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();


            var response = await _personApplicationService.GetByIdPerson(new GetByIdPersonDto { Id = id });

            if (!response.IsSuccessful || response.Result == null)
                return NotFound();

            var model = new PutPersonDto
            {
                Id = response.Result.Id,
                FirstName = response.Result.FirstName,
                LastName = response.Result.LastName,
                PhoneNumber = response.Result.PhoneNumber,
                BirthDate = response.Result.BirthDate,
                FilePath = response.Result.FilePath,
                UploadFile = response.Result.UploadFile

            };
            return View(model);
        }
        #endregion

        #region [- Edit(POST) -]
        // POST: /Person/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PutPersonDto putPersonDto)
        {
            if (!ModelState.IsValid)
                return View(putPersonDto);

            var response = await _personApplicationService.Put(putPersonDto);

            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return View(putPersonDto);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region [- Delete(GET) -]
        // GET: /Person/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var getByIdPersonDto = new GetByIdPersonDto { Id = id };
            var response = await _personApplicationService.GetByIdPerson(getByIdPersonDto);
            if (!response.IsSuccessful || response.Result == null)
            {
                return NotFound();
            }

            return View(response.Result);
        }
        #endregion

        #region [- Delete(POST) -]
        // POST: /Person/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var response = await _personApplicationService.Delete(new DeletePersonDto { Id = id });
            if (!response.IsSuccessful)
            {
                TempData["Error"] = response.ErrorMessage ?? "Failed to delete Contact.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
