using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IElasticClient elasticClient;

        public UsersController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        [HttpGet]
        public async Task<User> Get(string Name)
        {
            var res = await elasticClient.SearchAsync<User>(s => s
              .Index("users")
              .Query(q => q.Match(m => m.Field(f => f.Name).Query(Name))));

            return res?.Documents?.FirstOrDefault();
        }

        [HttpPost]
        public async Task<string> Post([FromBody] User value)
        {
            var res = await elasticClient.IndexAsync<User>(value, x => x.Index("users"));
            return res.Id;
        }
    }
}
