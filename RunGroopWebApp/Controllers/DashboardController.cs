using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;

    public DashboardController(IDashboardRepository dashboardRepository , IHttpContextAccessor httpContextAccessor,
        IPhotoService photoService)
    {
        _dashboardRepository = dashboardRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
    }

    private void MapUserEdit(AppUser user, ImageUploadResult photoResult, EditUserDashboardViewModel editVM)
    {
        user.Id = editVM.Id;
        user.Pace = editVM.Pace;
        user.Mileage = editVM.Mileage;
        user.ProfileImageUrl = photoResult.Url.ToString();
        user.City = editVM.City;
        user.State = editVM.State; 
    }
    
    public async Task<IActionResult> Index()
    {
        var userRaces = await _dashboardRepository.GetAllUserRaces();
        var userClubs = await _dashboardRepository.GetAllUserClubs();
        var dashboardViewModel = new DashboardViewModel()
        {
            Clubs = userClubs,
            Races = userRaces
        }; 
        return View(dashboardViewModel);
    }

    public async Task<IActionResult> EditUserProfile()
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _dashboardRepository.GetUserById(curUserId);
        if (user == null) return View("Error");
        var editUserViewModel = new EditUserDashboardViewModel()
        {
            Id = curUserId,
            City = user.City,
            Mileage = user.Mileage,
            Pace = user.Pace,
            ProfileImageUrl = user.ProfileImageUrl,
            State = user.State
        };
        return View(editUserViewModel);
    }
[HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit profile");
            return View("EditUserProfile", editVM);
        }

        AppUser user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);
        if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
        {
            var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
            MapUserEdit(user , photoResult , editVM);
            _dashboardRepository.Update(user);
            return RedirectToAction("Index", "Dashboard");
        }
        else
        {
            try
            {
                _photoService.DeletePhotoAsync(user.ProfileImageUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("" , "Could Not Delete Photo");
                return View(editVM );
            }
            var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
            MapUserEdit(user , photoResult , editVM);
            _dashboardRepository.Update(user);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}