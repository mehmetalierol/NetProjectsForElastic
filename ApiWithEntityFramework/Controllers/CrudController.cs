using Common;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithEntityFramework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ApiDbContext DbContext;

        public CrudController(ApiDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("GetUser")]
        public UserModel Get()
        {
            var user = DbContext.Users.FirstOrDefault(x => x.Title == "CL");

            return user;
        }

        [HttpPost("InsertMailLog")]
        public async Task<MailLog> InsertMailLog(UserModel userModel)
        {
            var model = new MailLog { UserId = userModel.Id };
            await DbContext.MailLogs.AddAsync(model);
            DbContext.SaveChanges();

            return model;
        }
    }
}