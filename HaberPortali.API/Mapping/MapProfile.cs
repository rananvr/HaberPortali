using AutoMapper;
using HaberPortali.API.DTOs;
using HaberPortali.API.Models;

namespace HaberPortali.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<News, NewsCreateDto>().ReverseMap();

        }
    }
}