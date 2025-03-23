using DotnetAppDemo.Service.Interfaces;
using DotnetAppDemo.Domain.Models;
using DotnetAppDemo.Repository.Data;

namespace DotnetAppDemo.Service.Services
{
    public class ItemService(AppDbContext db) : BaseService<Item>(db), IItemService
    {
    }
}
