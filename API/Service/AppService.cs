using API.Exceptions;
using API.Extensions;

namespace API.Service
{
    public abstract class AppService
    {
        private readonly IHttpContextAccessor _context;

        protected AppService(IHttpContextAccessor context)
        {
            _context = context;
        }

        protected int CurrentUserId
        {
            get
            {
                if (_context.HttpContext is null) throw new UnauthorizedException();
                return _context.HttpContext.User.GetUserId();
            }
        }
        protected string CurrentUserUsername
        {
            get
            {
                if (_context.HttpContext is null) throw new UnauthorizedException();
                return _context.HttpContext.User.GetUsername();
            }
        }
    }
}
