﻿using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void UpdateAsync(Category category);
        public Task<List<Category>> GetCategoriesByBookId(int bookId);
    }
}