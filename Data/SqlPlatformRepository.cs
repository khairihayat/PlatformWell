using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlatformWell.Models;

namespace PlatformWell.Data
{
    public class SqlPlatformRepository : IPlatformRepository
    {
        private readonly ApplicationContext _context;

        public SqlPlatformRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);
        }

        public void DeletePlatform(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Platforms.Remove(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.Include(x => x.Well).ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.Include(x => x.Well).FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePlatform(Platform platform)
        {
            
        }
    }
}