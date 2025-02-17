using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
           return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existtingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existtingRegion == null)
            {
                return null;
            }

            existtingRegion.Code = region.Code;
            existtingRegion.Name = region.Name;
            existtingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existtingRegion;

        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existtingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existtingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existtingRegion);
            await dbContext.SaveChangesAsync();
            return existtingRegion;

        }
    }
}
