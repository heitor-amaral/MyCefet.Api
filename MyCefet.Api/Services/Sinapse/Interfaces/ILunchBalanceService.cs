using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sinapse.Interfaces
{
    public interface ILunchBalanceService
    {
        Task<string> GetCurrentBalance(string jsessionid, string username, string password);
    }
}