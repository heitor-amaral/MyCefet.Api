using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sinapse.Interfaces
{
    public interface ILoginService
    {
        Task<string> Login(string username, string password);
    }
}