using LogicCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KvalDarbsCore.Controllers
{
    [Authorize]
    public class AuthorizedController : Controller
    {
    }
}
