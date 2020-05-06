using AutoMapper;
using webapi.DTO;
using webapi.Models;

namespace webapi.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegister, User>();
            CreateMap<AlbumForUpdate, Album>();
            CreateMap<Song, SongToReturn>();
            CreateMap<Album, AlbumToReturn>();
        }
    }
}
