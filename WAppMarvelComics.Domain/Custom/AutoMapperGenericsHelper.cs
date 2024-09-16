using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using WAppMarvelComics.Domain.Custom.Models;

namespace WAppMarvelComics.Domain.Custom
{
    public static class AutoMapperGenericsHelper
    {
        public static MapperConfiguration InitializeAutomapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Result, ComicModel>()
                .ForMember(
                    dest => dest.Description, act => act.MapFrom(
                        src => string.IsNullOrEmpty(src.Description) ? src.VariantDescription : src.Description
                    )
                )
                .ForMember(
                    dest => dest.Image, act => act.MapFrom(
                        src => src.Thumbnail != null ? $"{src.Thumbnail.Path}/standard_xlarge.{src.Thumbnail.Extension}" : string.Empty
                    )
                );

                cfg.CreateMap<Result, ComicDetailModel>().ForMember(
                    dest => dest.Description, act => act.MapFrom(
                        src => string.IsNullOrEmpty(src.Description) ? src.VariantDescription : src.Description
                    )
                )
                .ForMember(
                    dest => dest.Image, act => act.MapFrom(
                        src => src.Thumbnail != null ? $"{src.Thumbnail.Path}/clean.{src.Thumbnail.Extension}" : string.Empty
                    )
                )
                .ForMember(
                    dest => dest.PrintPrice, act => act.MapFrom(
                        src => src.Prices.Count != 0 && src.Prices.FirstOrDefault(p => p.Type == "printPrice") != null
                        ? src.Prices.First(p => p.Type == "printPrice").Price : 0
                    )
                )
                .ForMember(
                    dest => dest.PublishDate, act => act.MapFrom(
                        src => src.Dates.Count != 0 && src.Dates.FirstOrDefault(p => p.Type == "onsaleDate") != null
                            ? src.Dates.First(p => p.Type == "onsaleDate").Date : string.Empty
                    )
                )
                .ForMember(
                    dest => dest.Creators, act => act.MapFrom(
                        src => src.Creators != null && src.Creators.Items.Count != 0 
                            ? src.Creators.Items.ToList() : default
                    )
                );

                cfg.CreateMap<Item, CreatorR>();
            });
        }
    }
}