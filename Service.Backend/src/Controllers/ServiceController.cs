using Microsoft.AspNetCore.Mvc;
using Service.Backend.Model;

namespace Service.Backend.Controllers;

[ApiController]
public class ServiceController : ControllerBase {
    [HttpGet]
    [Route("api/v1/status")]
    public ServiceStatus Status() {
        return ServiceStatus.CreateOk();
    }
}