using Microsoft.AspNetCore.Mvc;
using ZeissTest.Interface;
using ZeissTest.Models;
using Serilog;

namespace ZeissTest.Controller;

/// <summary>
/// Provide a endpoint for user to get machine info.
/// </summary>
[ApiController]
public class MachineInfoController: ControllerBase
{
    private readonly ICacheService _cache;
    private readonly ILogger<MachineInfoController> _logger;
    
    public MachineInfoController(ICacheService cache, ILogger<MachineInfoController> logger)
    {
        _logger = logger;
        _cache = cache;
    }
    
    [HttpGet("MachineInfo")]
    public async Task<IActionResult> GetMachineInfo()
    {
        try
        {
            var machineInfo = await _cache.GetAsync(Consts.MachineInfoCacheKey) as MachineInfo;
                
            if (machineInfo == null)
                return NotFound();
            else
                return Ok(machineInfo);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error at {nameof(MachineInfoController)}: unable to get machine info.");
            return Problem(e.Message);
        }
    }
}