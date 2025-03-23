using System.Net;
using DotnetAppDemo.Domain.DTOs;
using DotnetAppDemo.Domain.Models;
using DotnetAppDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace DotnetAppDemo.API.Controllers
{
    public class ItemController(IItemService service, ILogger<ItemController> logger) : BaseController<Item>(service, logger)
    {
    }
}
