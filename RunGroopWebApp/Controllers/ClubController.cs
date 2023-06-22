using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClubController(IClubRepository clubRepository , IPhotoService photoService , IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _clubRepository = clubRepository;
        _photoService = photoService;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Club> clubs = await _clubRepository.GetAll();
        return View(clubs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);
        return View(club);
    }

    public IActionResult Create()
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var createClubViewModel = new CreateClubViewModel() { AppUserId = curUserId };
        return View(createClubViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel clubVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = result.Url.ToString(),
                AppUserId = clubVM.AppUserId,
                Address = new Address
                {
                    State = clubVM.Address.State,
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City
                }
            };
            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("" , "Photo Upload Failed");
        }
        return View(clubVM);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);
        if (club == null) return View("Error");
        var clubVM = new EditClubViewModel
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address, 
            URL = club.Image,
            ClubCategory = club.ClubCategory
        };
        return View(clubVM);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id , EditClubViewModel clubVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("","Failed To Edit Club");
            return View("Edit", clubVM);
        }

        var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
        if (userClub != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userClub.Image);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("" , "Could not Delete Photo.");
                return View(clubVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Address = clubVM.Address,
                Description = clubVM.Description,
                AddressId = clubVM.AddressId,
                Image = photoResult.Url.ToString(),
                Title = clubVM.Title,
                Id = clubVM.Id
            };
            _clubRepository.Update(club);
            return RedirectToAction("Index");
        }
        else
        {
            return View(clubVM);
        }
    }
    
    
}