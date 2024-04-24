using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services
{
    public interface IBaseService
    {
        ApiResponse responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
