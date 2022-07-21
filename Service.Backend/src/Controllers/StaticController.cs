using Microsoft.AspNetCore.Mvc;
using Service.Backend.Model;
using Service.Backend.Services;

namespace Service.Backend.Controllers;

[ApiController]
public class StaticController : ControllerBase {
    private readonly StaticService _staticService;

    public StaticController(
        StaticService staticService
    ) {
        _staticService = staticService;
    }

    [HttpGet]
    [Route("api/v1/static")]
    public StaticData GetStatic() {
        return _staticService.GetStatic();
    }
}