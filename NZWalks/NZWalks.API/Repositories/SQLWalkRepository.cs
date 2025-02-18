﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository    
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existtingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existtingWalk == null)
            {
                return null;
            }

            existtingWalk.Name = walk.Name;
            existtingWalk.Description = walk.Description;
            existtingWalk.LengthInKm = walk.LengthInKm;
            existtingWalk.WalkImageUrl = walk.WalkImageUrl;
            existtingWalk.RegionId = walk.RegionId;
            existtingWalk.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();

            return (existtingWalk);

        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existtingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existtingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existtingWalk);
            await dbContext.SaveChangesAsync();
            return existtingWalk;

        }
    }
}
