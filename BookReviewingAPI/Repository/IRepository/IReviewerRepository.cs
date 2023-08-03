﻿using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IReviewerRepository : IRepository<Reviewer>
    {
        void UpdateAsync(Reviewer reviewer);
    }
}
