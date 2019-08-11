using System.Threading.Tasks;
using MyCefet.Api.Models;

namespace MyCefet.Api.Interfaces
{
    public interface IReportGradesService
    {
        GradesReport GetAllGrades(string username, string password);
    }
}