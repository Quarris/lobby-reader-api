using LobbyReader.Common.Dtos;
using LobbyReader.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LobbyReader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService syncService;
        public SyncController(ISyncService syncService)
        {
            this.syncService = syncService;
        }
        [HttpPost("SyncLobbies")]
        public void SyncLobbies([FromBody] List<MessageDto> messages)
        {
            syncService.SyncLobbies(messages);
        }
        [HttpPost("SyncLineups")]
        public void SyncLineups([FromBody] List<MessageDto> messages)
        {
            syncService.SyncLineups(messages);
        }
    }
}
