using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace Data.Repository.Repository
{
    public class RepCombo : IRepCombo
    {
        private readonly OrderDbContext _db;
        public RepCombo(OrderDbContext db)
        {
            _db = db;
        }

        public async Task<List<Combo>> GetAllCombo()
        {
            return await _db.Combos.ToListAsync();
        }
        public async Task<Combo> CreateCombo(Combo combo)
        {
           
                _db.Combos.Add(combo);
                await _db.SaveChangesAsync();
                return combo;
        
        }

        public async Task<Combo> DeleteCombo(int id)
        {
                var Combo = await _db.Combos.FindAsync(id);
                _db.Combos.Remove(Combo);
                await _db.SaveChangesAsync();
                return Combo;
        }
    
        public async Task<Combo> GetById(int id)
        {
            return await _db.Combos.FindAsync(id);
        }

        public async Task<Combo> UpdateCombo(Combo combo)
        {
                _db.Combos.Update(combo);   
                await _db.SaveChangesAsync();
                return combo;
        }


        public async Task<List<Combo>> GetByName(string name)
        {
            var comboByName = await _db.Combos.
                                              Where(x => x.Name.ToLower().Contains(name.ToLower()))
                                              .ToListAsync();
            return comboByName;
        }


    }
}
