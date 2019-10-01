using System.Threading.Tasks;
using MyCefet.Api.Models;

namespace MyCefet.Api.Services.Sigaa.Interfaces
{
    public interface IReportGradesService
    {
        GradesReport GetAllGrades(string jsessionid, string username, string password);
    }
}