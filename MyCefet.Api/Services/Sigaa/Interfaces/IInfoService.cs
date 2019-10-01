using MyCefet.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sigaa.Interfaces
{
    public interface IInfoService
    {
        Task<Student> GetUserInfo(string jsessionid, string username, string password);
    }
}