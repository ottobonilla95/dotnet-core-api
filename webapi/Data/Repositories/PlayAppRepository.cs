using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Data.Interfaces;
using webapi.DTO;
using webapi.Models;

namespace webapi.Data.Repositories
{
    public class PlayAppRepository : IPlayAppRepository
    {
        DataContext _context;
        public PlayAppRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int userId) {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Id == userId);
        }

        public async Task<List<MenuItemDTO>> GetMenu()
        {
            var items = await
                (from menuitem in _context.MenuItem
                 join menuItemRelation in _context.MenuItemRelation on menuitem.Id equals menuItemRelation.MenuItemId into gj
                 from x in gj.DefaultIfEmpty()
                 select new MenuItemDTO
                 {
                     Id= menuitem.Id.ToString(),
                     Text= menuitem.Text,
                     Action= menuitem.Action,
                     Icon= menuitem.Icon,
                     MenuFatherId = (x == null ? null : x.MenuItemFatherId.ToString()),
                     sequence = (x == null ? null : x.sequence.ToString())
                 }
                ).ToListAsync();

            return items;
        }

        public async Task<List<Album>> GetAllAlbums()
        {
            return await _context.Albums.Include("User").ToListAsync();
        }

        public async Task<Album> GetAlbum(int Id)
        {
            return await _context.Albums.Include("Songs").Include("User").FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<Song> GetSong(int Id)
        {
            return await _context.Songs.FirstOrDefaultAsync(a => a.Id == Id);
        }
    }
}
