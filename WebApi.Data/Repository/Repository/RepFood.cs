using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace Data.Repository.Repository
{
    public class RepFood : IRepFood
    {
        private readonly OrderDbContext _db;
        public RepFood(OrderDbContext db)
        {
            _db = db;
        }
        public async Task<List<FastFoodItem>> GetAllFood()
        {
            return await _db.FastFoodItems.Include(x => x.Category).ToListAsync();
        }

        public async Task<FastFoodItem> CreateFood(FastFoodItem fastFoodItem)
        {
                 _db.FastFoodItems.Add(fastFoodItem);
                await _db.SaveChangesAsync();
                return fastFoodItem;
        }

        public async Task<FastFoodItem> DeleteFood(int id)
        {
                var FastFood = await _db.FastFoodItems.FindAsync(id);

                _db.FastFoodItems.Remove(FastFood);
                await _db.SaveChangesAsync();
                return FastFood;
        }

 
        public async Task<FastFoodItem> GetById(int id)
        {
            return await _db.FastFoodItems.FindAsync(id);
        }

        public async Task<FastFoodItem> UpdateFood(FastFoodItem fastFoodItem)
        {
                _db.FastFoodItems.Update(fastFoodItem);
                await _db.SaveChangesAsync();
                return fastFoodItem;
        }

        public async Task<FastFoodItem> GetByName(string name)
        {
            var FoodByName = await _db.FastFoodItems.FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return FoodByName;
        }
    }
}
