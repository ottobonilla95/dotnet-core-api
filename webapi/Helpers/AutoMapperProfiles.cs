using AutoMapper;
using webapi.DTO;
using webapi.Models;

namespace webapi.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserProfile>();
            CreateMap<UserForRegister, User>();

            CreateMap<AlbumForUpdate, Album>();
            CreateMap<Album, AlbumToReturn>();

            CreateMap<Song, SongToReturn>();
            CreateMap<SongForUpdate, Song>();

            
        }
    }
}
