using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.DTO;
using webapi.Models;

namespace webapi.Data.Interfaces
{
    public interface IPlayAppRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<List<Album>> GetAllAlbums();
        Task<Album> GetAlbum(int Id);
        Task<Song> GetSong(int Id);
        Task<List<MenuItemDTO>> GetMenu();

    }
}
