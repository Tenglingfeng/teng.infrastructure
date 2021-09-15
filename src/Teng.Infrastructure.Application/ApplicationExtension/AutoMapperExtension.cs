using System.Runtime.Serialization;
using AutoMapper;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Teng.Infrastructure.ApplicationExtension
{
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 转换成Dto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TDto ToDto<T, TDto>(this T entity) where T : Entity
        {
            var config = new MapperConfiguration(x => x.CreateMap<T, TDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<T, TDto>(entity);
        }

        /// <summary>
        /// 转换成Entity
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static T ToEntity<TDto, T>(this TDto dto) where T : EntityDto
        {
            var config = new MapperConfiguration(x => x.CreateMap<TDto, T>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDto, T>(dto);
        }
    }
}