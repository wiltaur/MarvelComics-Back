﻿using Ardalis.Specification.EntityFrameworkCore;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T>(AppDbContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}