using System.IO;
using System.Threading.Tasks;

namespace LizBot2._1.Services
{
    public interface IYouTubeService
    {
        Task<string> EnqueueSong(string searchTerm);
        Stream GetSong();
    }
}