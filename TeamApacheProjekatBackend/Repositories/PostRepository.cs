﻿using Microsoft.EntityFrameworkCore;
using TeamApacheProjekatBackend.Models;
using TeamApacheProjekatBackend.Repositories.Interfaces;

namespace TeamApacheProjekatBackend.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Post> _collection;

        public PostRepository(AppDbContext context)
        {
            _context = context;
            _collection = _context.Posts;
        }

        public async Task AddRate(Rating rating)
        {
            
           await _context.Rates.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePost(Post post)
        {
            try
            {
                await _collection.AddAsync(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task DeletePost(Post post)
        {
            _collection.Remove(post);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.Include(p => p.User).Include(p => p.PostLabels).OrderByDescending(p => p.CreatedTime).ToArrayAsync();
        }

        public async Task<Post> GetPostById(int id)
        {

            var post = await _context.Posts.FirstOrDefaultAsync
                (p => p.Id == id);

            if (post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<double?> GetRateAverage(int postId)
        {
            return await _context.Rates.Where(r => r.PostId == postId).Select(r => r.Rate).AverageAsync();
        }

        public  async Task<List<Rating>> GetRatingsByUserId(int postId, int userId)
        {
            return await _context.Rates.Where(r => r.PostId == postId && r.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetUsersPost(int userId)
        {
            var posts = await _collection.Include(p => p.User).Include(p => p.PostLabels).Where(p => p.UserId == userId).ToArrayAsync();
            return posts;
        }

        public async Task RemoveUserRate(int userId, int postId)
        {
            var rates = _context.Rates.Where(r => r.UserId == userId && r.PostId == postId).ToListAsync();
            foreach (var rate in await rates)
            {
                _context.Rates.Remove(rate);
                _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePost(Post post)
        {
            _collection.Update(post);
            await _context.SaveChangesAsync();
        }

      
    }
}
