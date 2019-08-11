using System.Threading.Tasks;

namespace MyCefet.Api.Interfaces
{
    public interface ILoginService
    {
        Task<string> GetJsession(string username, string password);
    }
}