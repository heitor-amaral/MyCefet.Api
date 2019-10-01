using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sigaa.Interfaces
{
    public interface ILoginService
    {
        Task<string> Login(string username, string password);
    }
}