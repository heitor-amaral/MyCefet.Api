using MyCefet.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCefet.Api.Interfaces
{
    public interface IInfoService
    {
        Task<Student> GetUserInfo(string username, string password);
    }
}