using Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.Models;
using Business;
using Microsoft.AspNetCore.Authorization;
using Project.Filters;
using Shared.ViewModels;
using System;
using Shared.Models.DataTransferObjects;


namespace Project.Controllers
{
    [CustomActionFilter]
    [Authorize]
    public class PhysicalPersonController : Controller
    {
        private IPersonsService _service;
        public PhysicalPersonController(IPersonsService personsService)
        {
            _service = personsService;
        }
        [AllowAnonymous]
        public IActionResult CreatePhysicalPerson()
        {
            return View(new CreatePersonCommandDto());
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("PhysicalPerson/CreatePhysicalPerson")]
        public async Task<IActionResult> CreatePhysicalPerson([FromForm] CreatePersonCommandDto physicalPerson)
        {
            int personId = await _service.CreatePersonAsync(physicalPerson);
            TempData["Message"] = $"Physical Person created with Id: {personId}";
            return RedirectToAction("GetAllPersons");
        }
        [HttpPost]
        [Route("PhysicalPerson/DeletePhysicalPerson/{personId?}")]
        public async Task<IActionResult> DeletePhysicalPerson(int personId, [FromServices] IFileService fileService)
        {
            try
            {
                var imagePath = await _service.DeletePersonAsync(personId);
                if (imagePath != null)
                {
                    fileService.DeleteFile(imagePath);
                }

                TempData["Message"] = $"Physical Person with ID: {personId} Deleted!";
                return RedirectToAction("GetAllPersons");
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = $"Person with ID: {personId} not found.";
            }
            catch (Exception)
            {
                TempData["Error"] = "An unexpected error occurred. Please try again.";
            }

            return RedirectToAction("UpdatePhysicalPerson", new { id = personId });
        }

        public async Task<IActionResult> UpdatePhysicalPerson(int id)
        {
            try
            {
                var person = await _service.GetPersonForUpdateAsync(id);
                return View(person);
            }
            catch (KeyNotFoundException)
            {
                TempData["Message"] = $"Physical Person with Id: {id} not found.";
                return RedirectToAction("GetAllPersons", "PhysicalPerson");
            }
        }
        [HttpPost]
        [Route("PhysicalPerson/UpdatePhysicalPerson")]
        public async Task<IActionResult> UpdatePhysicalPerson([FromForm] UpdatePersonCommandDto physicalPerson)
        {
            try
            {
                await _service.UpdatePersonAsync(physicalPerson);
                return RedirectToAction("GetAllPersons", "PhysicalPerson");
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError("Id", $"Person with Id: {physicalPerson.Id} not found.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }
            return View(physicalPerson);
        }
        [HttpPost]
        [Route("PhysicalPerson/AddRelation")]
        public async Task<IActionResult> AddRelation([FromForm] int fromId, [FromForm] int toId, [FromForm] RelationType relationType)
        {
            try
            {
                await _service.CreateRelationshipAsync(fromId, toId, relationType);
                return Ok(new { success = true, message = $"Added Relation {fromId} - {toId}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Invalid Id" });
            }
        }
        [HttpDelete]
        [Route("{fromId}/removeRelation/{toId}")]
        public async Task<IActionResult> RemoveRelation(int fromId, int toId)
        {
            await _service.RemoveRelationshipAsync(fromId, toId);
            return Ok($"Removed Relation {fromId} - {toId}");
        }
        [HttpGet]
        [Route("{personId}")]
        public async Task<IActionResult> GetFullInfo(int personId)
        {
            var Relationships = await _service.GetFullInfoByIdAsync(personId);
            return Ok(Relationships);
        }
        [HttpGet]
        [Route("PhysicalPerson/quickSearch/{query}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> QuickSearch(string query, int pageNumber, int pageSize)
        {
            var quickSearchResult = await _service.QuickSearchAsync(query, pageNumber, pageSize);
            return Ok(quickSearchResult);
        }
        [HttpGet]
        [Route("detailedSearch")]
        public async Task<IActionResult> DetailedSearch([FromQuery] DetailedSearchQueryDto detailedSearchDto)
        {
            var detailedSearchResult = await _service.DetailedSearchAsync(detailedSearchDto);
            return Ok(detailedSearchResult);
        }
        [HttpGet]
        [Route("relatedInfo")]
        public async Task<IActionResult> GetRelatedInfo()
        {
            var relatedInfo = await _service.GetRelatedInfoAsync();
            return Ok(relatedInfo);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var allPersons = await _service.GetAllPersonsAsync();
            return View(allPersons);
        }
        [HttpPost]
        [Route("{personId}/profilePicture")]
        public async Task<IActionResult> AddProfilePicture(int personId, IFormFile image, [FromServices] IFileService fileService)
        {
            if (image == null || image.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid image.";
                return RedirectToAction("Profile", new { personId });
            }

            var createdImage = await fileService.SaveFileAsync(image);
            var previousImage = await _service.SetProfilePictureAsync(personId, createdImage);

            if (!string.IsNullOrEmpty(previousImage))
            {
                fileService.DeleteFile(previousImage);
            }

            return RedirectToAction("Profile", new { id = personId });
        }
        [HttpGet]
        [Route("{personId}/profilePicture")]
        public async Task<IActionResult> GetProfilePicture(int personId, [FromServices] IFileService fileService)
        {
            var imagePath = await _service.GetProfilePictureAsync(personId);
            var image = await fileService.GetFileAsync(imagePath);
            return File(image, "image/jpeg");
        }
        [HttpPost]
        [Route("{personId}/profilePicture/remove")]
        public async Task<IActionResult> RemoveProfilePicture(int personId, [FromServices] IFileService fileService)
        {
            var imagePath = await _service.RemoveProfilePictureAsync(personId);
            if (!string.IsNullOrEmpty(imagePath))
            {
                fileService.DeleteFile(imagePath);
            }

            return RedirectToAction("Profile", new { id = personId });
        }
        public async Task<IActionResult> Profile(int id)
        {
            try
            {
                var person = await _service.GetFullInfoByIdAsync(id);
                return View(person);
            }
            catch (KeyNotFoundException)
            {
                TempData["Message"] = $"Physical Person with Id: {id} not found.";
                return RedirectToAction("GetAllPersons", "PhysicalPerson");
            }
        }

    }
}
