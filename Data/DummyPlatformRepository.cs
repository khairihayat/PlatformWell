using System;
using System.Collections.Generic;
using PlatformWell.Models;

namespace PlatformWell.Data
{
    public class DummyPlatformRepository : IPlatformRepository
    {
        public void CreatePlatform(Platform platform)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePlatform(Platform platform)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var platforms = new List<Platform>
            {
                new Platform{Id=1, UniqueName="Platform1", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-12), UpdatedAt=DateTime.Now.AddMonths(-12)},
                new Platform{Id=2, UniqueName="Platform2", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-11), UpdatedAt=DateTime.Now.AddMonths(-11)},
                new Platform{Id=3, UniqueName="Platform3", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=4, UniqueName="Platform4", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=5, UniqueName="Platform5", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=6, UniqueName="Platform6", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=7, UniqueName="Platform7", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=8, UniqueName="Platform8", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=9, UniqueName="Platform9", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=10, UniqueName="Platform10", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=11, UniqueName="Platform11", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=12, UniqueName="Platform12", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=13, UniqueName="Platform13", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=14, UniqueName="Platform14", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
                new Platform{Id=15, UniqueName="Platform15", Latitude=0, Longitude=0, CreatedAt= DateTime.Now.AddMonths(-10), UpdatedAt=DateTime.Now.AddMonths(-10)},
            };

            return platforms;
        }

        public Platform GetPlatformById(int id)
        {
            return new Platform{Id=1, UniqueName = "Platform1", Latitude = 0, Longitude = 0};
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePlatform(Platform platform)
        {
            throw new System.NotImplementedException();
        }
    }
}