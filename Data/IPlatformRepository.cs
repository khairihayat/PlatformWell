using System.Collections.Generic;
using PlatformWell.Models;

namespace PlatformWell.Data
{
    public interface IPlatformRepository
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform platform);
        void UpdatePlatform(Platform platform);
        void DeletePlatform(Platform platform);
    }
}