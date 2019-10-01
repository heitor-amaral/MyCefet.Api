using System.Threading.Tasks;
using MyCefet.Api.Models;

namespace MyCefet.Api.Services.Sigaa.Interfaces
{
    public interface IVirtualClassService
    {
        Task<GradesReport> GetAllGrades(string jsessionid, string username, string password);
    }
}